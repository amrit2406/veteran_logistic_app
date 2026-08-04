using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.Services;
using veteran_logistic.Reports.QueryBuilder.ViewModels;
using veteran_logistic.Reports.QueryBuilder.Export.Excel;
using veteran_logistic.Reports.QueryBuilder.Export.Pdf;
using veteran_logistic.Reports.QueryBuilder.Export.Csv;

namespace veteran_logistic.Reports.QueryBuilder.DependencyInjection;

/// <summary>
/// Extension methods for registering Query Builder feature infrastructure.
/// </summary>
public static class QueryBuilderServiceCollectionExtensions
{
    /// <summary>
    /// Adds Query Builder feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddQueryBuilder(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query engine
        services.AddScoped<IQueryEngine, QueryEngine>();

        // Export services
        services.AddScoped<IQueryBuilderExcelExporter, QueryBuilderExcelExporter>();
        services.AddScoped<IQueryBuilderPdfExporter, QueryBuilderPdfExporter>();
        services.AddScoped<IQueryBuilderCsvExporter, QueryBuilderCsvExporter>();

        // ViewModel
        services.AddTransient<QueryBuilderViewModel>();

        return services;
    }
}
