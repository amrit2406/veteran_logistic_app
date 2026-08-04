namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents a complete query definition in the query builder.
/// </summary>
public sealed class QueryDefinition
{
    /// <summary>
    /// Gets or sets the module ID to query.
    /// </summary>
    public string ModuleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the selected column field IDs.
    /// </summary>
    public List<string> SelectedColumns { get; set; } = new();

    /// <summary>
    /// Gets or sets the filter conditions.
    /// </summary>
    public List<QueryFilter> Filters { get; set; } = new();

    /// <summary>
    /// Gets or sets the sort conditions.
    /// </summary>
    public List<QuerySort> Sorts { get; set; } = new();

    /// <summary>
    /// Gets or sets the field ID to group by (if any).
    /// </summary>
    public string? GroupByFieldId { get; set; }

    /// <summary>
    /// Gets or sets the aggregate conditions.
    /// </summary>
    public List<QueryAggregate> Aggregates { get; set; } = new();

    /// <summary>
    /// Validates the query definition and returns any error messages.
    /// </summary>
    public List<string> Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(ModuleId))
        {
            errors.Add("Module must be selected.");
        }

        if (SelectedColumns.Count == 0)
        {
            errors.Add("At least one column must be selected.");
        }

        var duplicateColumns = SelectedColumns
            .GroupBy(c => c)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateColumns.Any())
        {
            errors.Add($"Duplicate columns selected: {string.Join(", ", duplicateColumns)}");
        }

        foreach (var filter in Filters)
        {
            if (string.IsNullOrWhiteSpace(filter.FieldId))
            {
                errors.Add("Filter field ID is required.");
            }

            if (filter.Operator == FilterOperator.Between)
            {
                if (string.IsNullOrWhiteSpace(filter.Value) || string.IsNullOrWhiteSpace(filter.Value2))
                {
                    errors.Add($"Between operator requires two values for field {filter.FieldId}.");
                }
            }
            else if (filter.Operator != FilterOperator.IsNull && 
                     filter.Operator != FilterOperator.IsNotNull &&
                     filter.Operator != FilterOperator.IsTrue && 
                     filter.Operator != FilterOperator.IsFalse)
            {
                if (string.IsNullOrWhiteSpace(filter.Value))
                {
                    errors.Add($"Filter value is required for field {filter.FieldId} with operator {filter.Operator}.");
                }
            }
        }

        foreach (var sort in Sorts)
        {
            if (string.IsNullOrWhiteSpace(sort.FieldId))
            {
                errors.Add("Sort field ID is required.");
            }
        }

        if (!string.IsNullOrWhiteSpace(GroupByFieldId) && Aggregates.Count == 0)
        {
            errors.Add("Grouping requires at least one aggregate.");
        }

        foreach (var aggregate in Aggregates)
        {
            if (string.IsNullOrWhiteSpace(aggregate.FieldId))
            {
                errors.Add("Aggregate field ID is required.");
            }
        }

        return errors;
    }
}
