using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.TdsReport.Contracts;
using veteran_logistic.Reports.TdsReport.Services;
using veteran_logistic.Reports.TdsReport.ViewModels;
using veteran_logistic.Reports.TdsReport.Export.Pdf;
using veteran_logistic.Reports.TdsReport.Export.Excel;

namespace veteran_logistic.Reports.TdsReport.DependencyInjection;

/// <summary>
/// Extension methods for registering TDS Report feature infrastructure.
/// </summary>
public static class TdsReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds TDS Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddTdsReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<ITdsReportQueryService, TdsReportQueryService>();

        // Export services
        services.AddScoped<ITdsReportPdfExporter, TdsReportPdfExporter>();
        services.AddScoped<ITdsReportExcelExporter, TdsReportExcelExporter>();

        // ViewModel
        services.AddTransient<TdsReportViewModel>();

        return services;
    }
}
