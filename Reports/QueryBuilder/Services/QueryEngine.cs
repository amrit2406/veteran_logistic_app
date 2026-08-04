using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using PartyBillRegisterEntity = VeteranLogistics.Data.Entities.Administration.PartyBillRegister;
using static veteran_logistic.Reports.QueryBuilder.Metadata.QueryMetadataProvider;

namespace veteran_logistic.Reports.QueryBuilder.Services;

/// <summary>
/// Implementation of the query engine for executing dynamic queries.
/// </summary>
public sealed class QueryEngine : IQueryEngine
{
    private const int MaxResultLimit = 10000;
    
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<QueryEngine> _logger;
    private readonly ConcurrentDictionary<string, Func<object, object?>> _propertyAccessors;
    private readonly ConcurrentDictionary<string, LambdaExpression> _filterExpressionCache;
    private readonly ConcurrentDictionary<string, LambdaExpression> _sortExpressionCache;
    private readonly ConcurrentDictionary<string, PropertyInfo> _propertyInfoCache;

    public QueryEngine(VeteranLogisticsDbContext dbContext, ILogger<QueryEngine> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _propertyAccessors = new ConcurrentDictionary<string, Func<object, object?>>();
        _filterExpressionCache = new ConcurrentDictionary<string, LambdaExpression>();
        _sortExpressionCache = new ConcurrentDictionary<string, LambdaExpression>();
        _propertyInfoCache = new ConcurrentDictionary<string, PropertyInfo>();
        
        // Pre-populate property info cache for known entity types
        PreloadPropertyInfoCache();
    }

    private void PreloadPropertyInfoCache()
    {
        var entityTypes = new[]
        {
            typeof(LoadingRegisterEntity),
            typeof(UnloadingRegisterEntity),
            typeof(PaymentRegisterEntity),
            typeof(PartyBillRegisterEntity)
        };

        foreach (var entityType in entityTypes)
        {
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                _propertyInfoCache.TryAdd($"{entityType.Name}.{property.Name}", property);
                
                // Also cache navigation property types
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    var navProperties = property.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var navProperty in navProperties)
                    {
                        _propertyInfoCache.TryAdd($"{property.PropertyType.Name}.{navProperty.Name}", navProperty);
                    }
                }
            }
        }
    }

    public async Task<QueryResult> ExecuteQueryAsync(
        QueryDefinition queryDefinition,
        string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("Executing query for module {ModuleId}", queryDefinition.ModuleId);

        var moduleMetadata = QueryMetadataProvider.GetModuleById(queryDefinition.ModuleId);
        if (moduleMetadata == null)
        {
            throw new ArgumentException($"Unknown module: {queryDefinition.ModuleId}");
        }

        var query = GetBaseQuery(queryDefinition.ModuleId);

        query = ApplyFilters(query, queryDefinition.Filters, moduleMetadata);
        query = ApplySearch(query, searchText, queryDefinition.ModuleId, moduleMetadata);

        if (!string.IsNullOrWhiteSpace(queryDefinition.GroupByFieldId))
        {
            var groupedResult = await ExecuteGroupedQueryAsync(
                query, 
                queryDefinition, 
                moduleMetadata, 
                cancellationToken);
            
            stopwatch.Stop();
            groupedResult.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
            _logger.LogInformation("Grouped query executed in {ElapsedMs}ms with {Count} records", 
                stopwatch.ElapsedMilliseconds, groupedResult.TotalCount);
            
            return groupedResult;
        }

        query = ApplySorting(query, queryDefinition.Sorts, moduleMetadata);

        // Limit results to prevent memory issues
        var takeMethod = typeof(System.Linq.Queryable)
            .GetMethods()
            .FirstOrDefault(m => m.Name == "Take" && m.GetParameters().Length == 2);
        
        if (takeMethod != null)
        {
            query = (IQueryable)takeMethod.MakeGenericMethod(query.ElementType)
                .Invoke(null, new object[] { query, MaxResultLimit })!;
        }

        var items = await ProjectToResultItems(
            query, 
            queryDefinition.SelectedColumns, 
            moduleMetadata,
            queryDefinition.ModuleId,
            cancellationToken).ConfigureAwait(false);

        stopwatch.Stop();

        var result = new QueryResult
        {
            Items = items,
            TotalCount = items.Count,
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
            ColumnHeaders = queryDefinition.SelectedColumns
        };

        _logger.LogInformation("Query executed in {ElapsedMs}ms with {Count} records", 
            stopwatch.ElapsedMilliseconds, result.TotalCount);

        return result;
    }

    private IQueryable GetBaseQuery(string moduleId)
    {
        return moduleId switch
        {
            LoadingModuleId => _dbContext.LoadingRegisters
                .AsNoTracking()
                .AsSplitQuery(),
            
            UnloadingModuleId => _dbContext.UnloadingRegisters
                .AsNoTracking()
                .AsSplitQuery(),
            
            PaymentModuleId => _dbContext.PaymentRegisters
                .AsNoTracking()
                .AsSplitQuery(),
            
            PartyBillingModuleId => _dbContext.PartyBillRegisters
                .AsNoTracking()
                .AsSplitQuery(),
            
            _ => throw new ArgumentException($"Unknown module: {moduleId}")
        };
    }

    private IQueryable ApplyFilters(IQueryable query, List<QueryFilter> filters, ModuleMetadata moduleMetadata)
    {
        foreach (var filter in filters)
        {
            var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == filter.FieldId);
            if (field == null) continue;

            query = ApplyFilter(query, filter, field);
        }

        return query;
    }

    private IQueryable ApplyFilter(IQueryable query, QueryFilter filter, FieldMetadata field)
    {
        var cacheKey = $"{query.ElementType.Name}.{field.FieldId}.{filter.Operator}.{filter.Value}.{filter.Value2}";
        
        var lambda = (LambdaExpression?)_filterExpressionCache.GetOrAdd(cacheKey, _ =>
        {
            var parameter = Expression.Parameter(query.ElementType, "x");
            var propertyAccess = GetPropertyExpression(parameter, field.PropertyPath);

            Expression? filterExpression = null;

            try
            {
                filterExpression = field.DataType switch
                {
                    FieldDataType.Text => BuildTextFilterExpression(propertyAccess, filter),
                    FieldDataType.Number => BuildNumberFilterExpression(propertyAccess, filter),
                    FieldDataType.Date => BuildDateFilterExpression(propertyAccess, filter),
                    FieldDataType.Boolean => BuildBooleanFilterExpression(propertyAccess, filter),
                    _ => null
                };

                if (filterExpression != null)
                {
                    return Expression.Lambda(filterExpression, parameter);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to build filter expression for field {FieldId} with operator {Operator}", 
                    filter.FieldId, filter.Operator);
            }

            return null!;
        });

        if (lambda == null)
        {
            return query;
        }

        try
        {
            var whereMethod = typeof(Queryable).GetMethods()
                .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 2)?
                .MakeGenericMethod(query.ElementType);

            if (whereMethod != null)
            {
                query = (IQueryable)whereMethod.Invoke(null, new object[] { query, lambda })!;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to apply filter for field {FieldId} with operator {Operator}", 
                filter.FieldId, filter.Operator);
        }

        return query;
    }

    private Expression BuildTextFilterExpression(MemberExpression propertyAccess, QueryFilter filter)
    {
        var constant = Expression.Constant(filter.Value ?? string.Empty, typeof(string));

        return filter.Operator switch
        {
            FilterOperator.Contains => Expression.Call(typeof(EF), "Like", null, propertyAccess, 
                Expression.Constant($"%{filter.Value}%")),
            FilterOperator.StartsWith => Expression.Call(typeof(EF), "Like", null, propertyAccess, 
                Expression.Constant($"{filter.Value}%")),
            FilterOperator.EndsWith => Expression.Call(typeof(EF), "Like", null, propertyAccess, 
                Expression.Constant($"%{filter.Value}")),
            FilterOperator.Equals => Expression.Equal(propertyAccess, constant),
            FilterOperator.NotEquals => Expression.NotEqual(propertyAccess, constant),
            _ => Expression.Equal(propertyAccess, constant)
        };
    }

    private Expression BuildNumberFilterExpression(MemberExpression propertyAccess, QueryFilter filter)
    {
        if (!decimal.TryParse(filter.Value, out var value))
        {
            return Expression.Constant(true);
        }

        var constant = Expression.Constant(value);
        var propertyValue = Expression.Convert(propertyAccess, typeof(decimal));

        return filter.Operator switch
        {
            FilterOperator.Equals => Expression.Equal(propertyValue, constant),
            FilterOperator.NotEquals => Expression.NotEqual(propertyValue, constant),
            FilterOperator.GreaterThan => Expression.GreaterThan(propertyValue, constant),
            FilterOperator.LessThan => Expression.LessThan(propertyValue, constant),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyValue, constant),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyValue, constant),
            FilterOperator.Between => BuildBetweenExpression(propertyValue, filter),
            _ => Expression.Equal(propertyValue, constant)
        };
    }

    private Expression BuildBetweenExpression(Expression propertyValue, QueryFilter filter)
    {
        if (!decimal.TryParse(filter.Value, out var value1) || !decimal.TryParse(filter.Value2, out var value2))
        {
            return Expression.Constant(true);
        }

        var minDate = value1 < value2 ? value1 : value2;
        var maxDate = value1 < value2 ? value2 : value1;

        var min = Expression.Constant(minDate);
        var max = Expression.Constant(maxDate);

        var greaterThanOrEqual = Expression.GreaterThanOrEqual(propertyValue, min);
        var lessThanOrEqual = Expression.LessThanOrEqual(propertyValue, max);

        return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
    }

    private Expression BuildDateFilterExpression(MemberExpression propertyAccess, QueryFilter filter)
    {
        if (!DateTime.TryParse(filter.Value, out var value))
        {
            return Expression.Constant(true);
        }

        var constant = Expression.Constant(value.Date);
        var propertyValue = Expression.Convert(propertyAccess, typeof(DateTime));
        var propertyDate = Expression.Call(propertyValue, "Date", null);

        return filter.Operator switch
        {
            FilterOperator.Equals => Expression.Equal(propertyDate, constant),
            FilterOperator.NotEquals => Expression.NotEqual(propertyDate, constant),
            FilterOperator.Before => Expression.LessThan(propertyDate, constant),
            FilterOperator.After => Expression.GreaterThan(propertyDate, constant),
            FilterOperator.Between => BuildDateBetweenExpression(propertyDate, filter),
            _ => Expression.Equal(propertyDate, constant)
        };
    }

    private Expression BuildDateBetweenExpression(Expression propertyDate, QueryFilter filter)
    {
        if (!DateTime.TryParse(filter.Value, out var value1) || !DateTime.TryParse(filter.Value2, out var value2))
        {
            return Expression.Constant(true);
        }

        var minDate = value1 < value2 ? value1.Date : value2.Date;
        var maxDate = value1 < value2 ? value2.Date : value1.Date;

        var min = Expression.Constant(minDate);
        var max = Expression.Constant(maxDate);

        var greaterThanOrEqual = Expression.GreaterThanOrEqual(propertyDate, min);
        var lessThanOrEqual = Expression.LessThanOrEqual(propertyDate, max);

        return Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);
    }

    private Expression BuildBooleanFilterExpression(MemberExpression propertyAccess, QueryFilter filter)
    {
        var propertyValue = Expression.Convert(propertyAccess, typeof(bool));

        return filter.Operator switch
        {
            FilterOperator.IsTrue => Expression.Equal(propertyValue, Expression.Constant(true)),
            FilterOperator.IsFalse => Expression.Equal(propertyValue, Expression.Constant(false)),
            FilterOperator.IsNull => Expression.Equal(propertyAccess, Expression.Constant(null)),
            FilterOperator.IsNotNull => Expression.NotEqual(propertyAccess, Expression.Constant(null)),
            _ => Expression.Equal(propertyValue, Expression.Constant(true))
        };
    }

    private IQueryable ApplySearch(IQueryable query, string? searchText, string moduleId, ModuleMetadata moduleMetadata)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return query;
        }

        var cacheKey = $"{moduleId}.{searchText}";
        
        var lambda = (LambdaExpression?)_filterExpressionCache.GetOrAdd(cacheKey, _ =>
        {
            var searchPattern = $"%{searchText}%";
            var parameter = Expression.Parameter(query.ElementType, "x");

            var searchConditions = new List<Expression>();

            foreach (var field in moduleMetadata.Fields.Where(f => f.DataType == FieldDataType.Text))
            {
                try
                {
                    var propertyAccess = GetPropertyExpression(parameter, field.PropertyPath);
                    var constant = Expression.Constant(searchPattern, typeof(string));
                    var likeCall = Expression.Call(typeof(EF), "Like", null, propertyAccess, constant);
                    var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
                    var condition = Expression.AndAlso(nullCheck, likeCall);
                    searchConditions.Add(condition);
                }
                catch
                {
                    continue;
                }
            }

            if (searchConditions.Count == 0)
            {
                return null!;
            }

            var combinedCondition = searchConditions.Aggregate((acc, cond) => Expression.OrElse(acc, cond));
            return Expression.Lambda(combinedCondition, parameter);
        });

        if (lambda == null)
        {
            return query;
        }

        var whereMethod = typeof(Queryable).GetMethods()
            .FirstOrDefault(m => m.Name == "Where" && m.GetParameters().Length == 2)?
            .MakeGenericMethod(query.ElementType);

        if (whereMethod != null)
        {
            query = (IQueryable)whereMethod.Invoke(null, new object[] { query, lambda })!;
        }

        return query;
    }

    private IQueryable ApplySorting(IQueryable query, List<QuerySort> sorts, ModuleMetadata moduleMetadata)
    {
        if (sorts.Count == 0)
        {
            return query;
        }

        var sortedSorts = sorts.OrderBy(s => s.Priority).ToList();
        var parameter = Expression.Parameter(query.ElementType, "x");

        bool firstSort = true;

        foreach (var sort in sortedSorts)
        {
            var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == sort.FieldId);
            if (field == null) continue;

            var cacheKey = $"{query.ElementType.Name}.{field.FieldId}.{sort.Ascending}";
            
            var lambda = (LambdaExpression?)_sortExpressionCache.GetOrAdd(cacheKey, _ =>
            {
                try
                {
                    var propertyAccess = GetPropertyExpression(parameter, field.PropertyPath);
                    return Expression.Lambda(propertyAccess, parameter);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to build sort expression for field {FieldId}", sort.FieldId);
                    return null!;
                }
            });

            if (lambda == null)
            {
                continue;
            }

            try
            {
                string methodName = firstSort 
                    ? (sort.Ascending ? "OrderBy" : "OrderByDescending")
                    : (sort.Ascending ? "ThenBy" : "ThenByDescending");

                var method = typeof(Queryable).GetMethods()
                    .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2)?
                    .MakeGenericMethod(query.ElementType, lambda.ReturnType);

                if (method != null)
                {
                    query = (IQueryable)method.Invoke(null, new object[] { query, lambda })!;
                    firstSort = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to apply sort for field {FieldId}", sort.FieldId);
            }
        }

        return query;
    }

    private MemberExpression GetPropertyExpression(Expression parameter, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        var expression = parameter;

        foreach (var property in properties)
        {
            expression = Expression.PropertyOrField(expression, property);
        }

        return (MemberExpression)expression;
    }

    private async Task<List<QueryResultItem>> ProjectToResultItems(
        IQueryable query, 
        List<string> selectedColumns, 
        ModuleMetadata moduleMetadata,
        string moduleId,
        CancellationToken cancellationToken)
    {
        // Add includes only for navigation properties that are actually needed
        var requiredIncludes = GetRequiredIncludes(selectedColumns, moduleMetadata);
        query = ApplyIncludes(query, requiredIncludes, moduleId);

        var items = new List<QueryResultItem>();
        var columnFields = selectedColumns
            .Select(colId => moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == colId))
            .Where(f => f != null)
            .ToList();

        try
        {
            // UseToListAsync for better async support
            var toArrayMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "ToArrayAsync" && m.GetParameters().Length == 2);

            if (toArrayMethod != null)
            {
                var genericMethod = toArrayMethod.MakeGenericMethod(query.ElementType);
                var task = (Task)genericMethod.Invoke(null, new object[] { query, cancellationToken })!;
                await task.ConfigureAwait(false);
                
                var results = (dynamic)task.GetType().GetProperty("Result")!.GetValue(task)!;
                
                foreach (var result in results)
                {
                    var item = new QueryResultItem();

                    foreach (var field in columnFields.Where(f => f != null))
                    {
                        try
                        {
                            var value = GetPropertyValue(result, field!.PropertyPath);
                            item.SetValue(field!.FieldId, value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to get property value for {PropertyPath}", field!.PropertyPath);
                            item.SetValue(field!.FieldId, null);
                        }
                    }

                    items.Add(item);
                }
            }
            else
            {
                // Fallback to synchronous execution
                var results = await Task.Run(() => 
                {
                    var enumerable = (System.Collections.IEnumerable)query;
                    var list = new System.Collections.ArrayList();
                    foreach (var item in enumerable)
                    {
                        list.Add(item);
                    }
                    return list.Cast<object>().ToList();
                }, cancellationToken).ConfigureAwait(false);

                foreach (var result in results)
                {
                    var item = new QueryResultItem();

                    foreach (var field in columnFields.Where(f => f != null))
                    {
                        try
                        {
                            var value = GetPropertyValue(result, field!.PropertyPath);
                            item.SetValue(field!.FieldId, value);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to get property value for {PropertyPath}", field!.PropertyPath);
                            item.SetValue(field!.FieldId, null);
                        }
                    }

                    items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute query and project results");
            throw;
        }

        return items;
    }

    private HashSet<string> GetRequiredIncludes(List<string> selectedColumns, ModuleMetadata moduleMetadata)
    {
        var includes = new HashSet<string>();
        
        foreach (var columnId in selectedColumns)
        {
            var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
            if (field == null) continue;

            var pathParts = field.PropertyPath.Split('.');
            if (pathParts.Length > 1)
            {
                // Include the navigation property (first part)
                includes.Add(pathParts[0]);
            }
        }

        return includes;
    }

    private IQueryable ApplyIncludes(IQueryable query, HashSet<string> includes, string moduleId)
    {
        // Use string-based includes for simplicity and reliability
        if (includes.Count == 0)
        {
            return query;
        }

        try
        {
            var includeMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .FirstOrDefault(m => m.Name == "Include" && 
                                        m.GetParameters().Length == 2 && 
                                        m.GetParameters()[1].ParameterType == typeof(string));

            if (includeMethod != null)
            {
                foreach (var include in includes)
                {
                    query = (IQueryable)includeMethod.MakeGenericMethod(query.ElementType)
                        .Invoke(null, new object[] { query, include })!;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to apply includes");
        }

        return query;
    }

    private object? GetPropertyValue(object obj, string propertyPath)
    {
        if (obj == null) return null;

        var accessor = _propertyAccessors.GetOrAdd($"{obj.GetType().Name}.{propertyPath}", path => 
        {
            return CompilePropertyAccessor(obj.GetType(), path);
        });

        return accessor(obj);
    }

    private Func<object, object?> CompilePropertyAccessor(Type type, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        var parameter = Expression.Parameter(typeof(object), "obj");
        Expression expression = Expression.Convert(parameter, type);

        foreach (var property in properties)
        {
            var currentType = expression.Type;
            
            // Use cached PropertyInfo instead of runtime reflection
            var cacheKey = $"{currentType.Name}.{property}";
            var propertyInfo = _propertyInfoCache.GetOrAdd(cacheKey, _ => 
            {
                // Only fall back to reflection if not in cache
                return currentType.GetProperty(property) ?? throw new InvalidOperationException($"Property '{property}' not found on type '{currentType.Name}'");
            });
            
            if (propertyInfo == null)
            {
                return _ => null;
            }
            expression = Expression.Property(expression, propertyInfo!);
        }

        var conversion = Expression.Convert(expression, typeof(object));
        var lambda = Expression.Lambda<Func<object, object?>>(conversion, parameter);
        return lambda.Compile();
    }

    private async Task<QueryResult> ExecuteGroupedQueryAsync(
        IQueryable query,
        QueryDefinition queryDefinition,
        ModuleMetadata moduleMetadata,
        CancellationToken cancellationToken)
    {
        var groupField = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == queryDefinition.GroupByFieldId);
        if (groupField == null)
        {
            throw new ArgumentException($"Group field not found: {queryDefinition.GroupByFieldId}");
        }

        // Apply sorting before grouping
        query = ApplySorting(query, queryDefinition.Sorts, moduleMetadata);

        // Limit results before grouping to prevent memory issues
        var takeMethod = typeof(System.Linq.Queryable)
            .GetMethods()
            .FirstOrDefault(m => m.Name == "Take" && m.GetParameters().Length == 2);
        
        if (takeMethod != null)
        {
            query = (IQueryable)takeMethod.MakeGenericMethod(query.ElementType)
                .Invoke(null, new object[] { query, MaxResultLimit })!;
        }

        // Materialize with projection to avoid reflection
        var projectedResults = await ProjectToResultItems(
            query,
            queryDefinition.SelectedColumns.Union(new[] { groupField.FieldId }).ToList(),
            moduleMetadata,
            queryDefinition.ModuleId,
            cancellationToken).ConfigureAwait(false);

        // Client-side grouping (acceptable for limited result set)
        var groupedData = projectedResults
            .GroupBy(r => r.GetValue(groupField.FieldId))
            .ToList();

        var items = new List<QueryResultItem>();

        foreach (var group in groupedData)
        {
            var item = new QueryResultItem();
            item.SetValue(groupField.FieldId, group.Key);

            foreach (var aggregate in queryDefinition.Aggregates)
            {
                var aggField = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == aggregate.FieldId);
                if (aggField == null) continue;

                var aggregateValue = CalculateAggregateFromResults(group.ToList(), aggField.FieldId, aggregate.AggregateType);
                item.SetValue(aggregate.DisplayName, aggregateValue);
            }

            foreach (var columnId in queryDefinition.SelectedColumns.Where(c => c != groupField.FieldId))
            {
                var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
                if (field == null) continue;

                var firstItem = group.FirstOrDefault();
                if (firstItem != null)
                {
                    var firstValue = firstItem.GetValue(field.FieldId);
                    item.SetValue(field.FieldId, firstValue);
                }
            }

            items.Add(item);
        }

        return new QueryResult
        {
            Items = items,
            TotalCount = items.Count,
            ColumnHeaders = queryDefinition.SelectedColumns
        };
    }

    private object? CalculateAggregateFromResults(List<QueryResultItem> items, string fieldId, AggregateType aggregateType)
    {
        var values = items
            .Select(item => item.GetValue(fieldId))
            .Where(v => v != null)
            .ToList();

        if (values.Count == 0)
        {
            return aggregateType == AggregateType.Count ? 0 : null;
        }

        return aggregateType switch
        {
            AggregateType.Count => values.Count,
            AggregateType.Sum => SafeSum(values),
            AggregateType.Average => SafeAverage(values),
            AggregateType.Minimum => SafeMin(values),
            AggregateType.Maximum => SafeMax(values),
            _ => null
        };
    }

    private decimal SafeSum(List<object?> values)
    {
        try
        {
            checked
            {
                return values.Sum(v => ConvertToDecimal(v));
            }
        }
        catch (OverflowException)
        {
            _logger.LogWarning("Sum overflow detected in query aggregation");
            return decimal.MaxValue;
        }
    }

    private decimal? SafeAverage(List<object?> values)
    {
        try
        {
            return values.Average(v => ConvertToDecimal(v));
        }
        catch (OverflowException)
        {
            _logger.LogWarning("Average overflow detected in query aggregation");
            return null;
        }
    }

    private decimal? SafeMin(List<object?> values)
    {
        try
        {
            return values.Min(v => ConvertToDecimal(v));
        }
        catch (OverflowException)
        {
            _logger.LogWarning("Min overflow detected in query aggregation");
            return null;
        }
    }

    private decimal? SafeMax(List<object?> values)
    {
        try
        {
            return values.Max(v => ConvertToDecimal(v));
        }
        catch (OverflowException)
        {
            _logger.LogWarning("Max overflow detected in query aggregation");
            return null;
        }
    }

    private decimal ConvertToDecimal(object? value)
    {
        if (value == null) return 0;
        
        try
        {
            return Convert.ToDecimal(value);
        }
        catch (FormatException)
        {
            _logger.LogWarning("Failed to convert value to decimal: {Value}", value);
            return 0;
        }
        catch (InvalidCastException)
        {
            _logger.LogWarning("Invalid cast to decimal for value: {Value}", value);
            return 0;
        }
    }
}
