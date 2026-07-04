using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Services;

namespace veteran_logistic.Authorization.DependencyInjection;

/// <summary>
/// Extension methods for registering authorization services.
/// </summary>
public static class AuthorizationServiceCollectionExtensions
{
    /// <summary>
    /// Adds authorization services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<IRoleAuthorizationService, RoleAuthorizationService>();

        return services;
    }
}
