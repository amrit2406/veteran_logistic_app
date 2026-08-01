using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.PartyBillingReport.Contracts;
using veteran_logistic.Reports.PartyBillingReport.Services;
using veteran_logistic.Reports.PartyBillingReport.ViewModels;
using veteran_logistic.Reports.PartyBillingReport.Export.Pdf;
using veteran_logistic.Reports.PartyBillingReport.Export.Excel;

namespace veteran_logistic.Reports.PartyBillingReport.DependencyInjection;

/// <summary>
/// Extension methods for registering Party Billing Report feature infrastructure.
/// </summary>
public static class PartyBillingReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds Party Billing Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPartyBillingReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<IPartyBillingReportQueryService, PartyBillingReportQueryService>();

        // Export services
        services.AddScoped<IPartyBillingReportPdfExporter, PartyBillingReportPdfExporter>();
        services.AddScoped<IPartyBillingReportExcelExporter, PartyBillingReportExcelExporter>();

        // ViewModel
        services.AddTransient<PartyBillingReportViewModel>();

        return services;
    }
}
