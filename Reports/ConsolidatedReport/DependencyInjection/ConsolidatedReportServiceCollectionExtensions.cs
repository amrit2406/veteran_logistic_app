using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.ConsolidatedReport.Contracts;
using veteran_logistic.Reports.ConsolidatedReport.Services;
using veteran_logistic.Reports.ConsolidatedReport.ViewModels;
using veteran_logistic.Reports.ConsolidatedReport.Export.Pdf;
using veteran_logistic.Reports.ConsolidatedReport.Export.Excel;

namespace veteran_logistic.Reports.ConsolidatedReport.DependencyInjection;

/// <summary>
/// Extension methods for registering Consolidated Report feature infrastructure.
/// </summary>
public static class ConsolidatedReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds Consolidated Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddConsolidatedReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<IConsolidatedReportQueryService, ConsolidatedReportQueryService>();

        // Export services
        services.AddScoped<IConsolidatedReportPdfExporter, ConsolidatedReportPdfExporter>();
        services.AddScoped<IConsolidatedReportExcelExporter, ConsolidatedReportExcelExporter>();

        // ViewModel
        services.AddTransient<ConsolidatedReportViewModel>();

        return services;
    }
}
