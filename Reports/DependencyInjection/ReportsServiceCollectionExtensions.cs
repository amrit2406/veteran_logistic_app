using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.LoadingReport.DependencyInjection;

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

        return services;
    }
}
