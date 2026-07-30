using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.LoadingReport.Contracts;
using veteran_logistic.Reports.LoadingReport.Services;
using veteran_logistic.Reports.LoadingReport.ViewModels;
using veteran_logistic.Reports.LoadingReport.Export.Pdf;
using veteran_logistic.Reports.LoadingReport.Export.Excel;

namespace veteran_logistic.Reports.LoadingReport.DependencyInjection;

/// <summary>
/// Extension methods for registering Loading Report feature infrastructure.
/// </summary>
public static class LoadingReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds Loading Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLoadingReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<ILoadingReportQueryService, LoadingReportQueryService>();

        // Export services
        services.AddScoped<ILoadingReportPdfExporter, LoadingReportPdfExporter>();
        services.AddScoped<ILoadingReportExcelExporter, LoadingReportExcelExporter>();

        // ViewModel
        services.AddTransient<LoadingReportViewModel>();

        return services;
    }
}
