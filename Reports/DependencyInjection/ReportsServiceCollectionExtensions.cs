using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.LoadingReport.DependencyInjection;
using veteran_logistic.Reports.UnloadingReport.DependencyInjection;
using veteran_logistic.Reports.PaymentReport.DependencyInjection;
using veteran_logistic.Reports.PartyBillingReport.DependencyInjection;
using veteran_logistic.Reports.TdsReport.DependencyInjection;

namespace veteran_logistic.Reports.DependencyInjection;

/// <summary>
/// Extension methods for registering Reports feature infrastructure.
/// </summary>
public static class ReportsServiceCollectionExtensions
{
    /// <summary>
    /// Adds Reports feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddReports(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Loading Report
        services.AddLoadingReport();

        // Unloading Report
        services.AddUnloadingReport();

        // Payment Report
        services.AddPaymentReport();

        // Party Billing Report
        services.AddPartyBillingReport();

        // TDS Report
        services.AddTdsReport();

        return services;
    }
}
