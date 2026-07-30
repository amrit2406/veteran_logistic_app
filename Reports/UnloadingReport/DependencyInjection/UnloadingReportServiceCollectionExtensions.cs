using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.UnloadingReport.Contracts;
using veteran_logistic.Reports.UnloadingReport.Services;
using veteran_logistic.Reports.UnloadingReport.ViewModels;
using veteran_logistic.Reports.UnloadingReport.Export.Pdf;
using veteran_logistic.Reports.UnloadingReport.Export.Excel;

namespace veteran_logistic.Reports.UnloadingReport.DependencyInjection;

/// <summary>
/// Extension methods for registering Unloading Report feature infrastructure.
/// </summary>
public static class UnloadingReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds Unloading Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddUnloadingReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<IUnloadingReportQueryService, UnloadingReportQueryService>();

        // Export services
        services.AddScoped<IUnloadingReportPdfExporter, UnloadingReportPdfExporter>();
        services.AddScoped<IUnloadingReportExcelExporter, UnloadingReportExcelExporter>();

        // ViewModel
        services.AddTransient<UnloadingReportViewModel>();

        return services;
    }
}
