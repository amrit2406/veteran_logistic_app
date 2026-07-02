using Microsoft.Extensions.DependencyInjection;

namespace VeteranLogistics.Shared.DependencyInjection;

/// <summary>
/// Shared project DI registrations. Currently a placeholder for foundational services.
/// Add common infrastructure registrations here when implementations are provided.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers shared infrastructure services.
    /// Currently no concrete services are registered; this method exists for future extension.
    /// </summary>
    public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
    {
        // Example for future services (do not register implementations that don't exist):
        // services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
