using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.DOStatusReport.Contracts;
using veteran_logistic.Reports.DOStatusReport.Services;
using veteran_logistic.Reports.DOStatusReport.ViewModels;
using veteran_logistic.Reports.DOStatusReport.Export.Pdf;
using veteran_logistic.Reports.DOStatusReport.Export.Excel;

namespace veteran_logistic.Reports.DOStatusReport.DependencyInjection;

/// <summary>
/// Extension methods for registering DO Status Report feature infrastructure.
/// </summary>
public static class DOStatusReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds DO Status Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddDOStatusReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<IDOStatusReportQueryService, DOStatusReportQueryService>();

        // Export services
        services.AddScoped<IDOStatusReportPdfExporter, DOStatusReportPdfExporter>();
        services.AddScoped<IDOStatusReportExcelExporter, DOStatusReportExcelExporter>();

        // ViewModel
        services.AddTransient<DOStatusReportViewModel>();

        return services;
    }
}
