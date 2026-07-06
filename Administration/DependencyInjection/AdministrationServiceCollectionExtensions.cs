using Microsoft.Extensions.DependencyInjection;

namespace veteran_logistic.Administration.DependencyInjection;

/// <summary>
/// Extension methods for registering Administration feature infrastructure.
/// </summary>
public static class AdministrationServiceCollectionExtensions
{
    /// <summary>
    /// Adds Administration feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddAdministration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Administration feature infrastructure registration will be added in future phases.
        // This method establishes the feature's presence in the DI container without registering services.

        return services;
    }
}
