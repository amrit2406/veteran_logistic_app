using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using veteran_logistic.Authentication.Session;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers authentication infrastructure services.
/// </summary>
public static class AuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authentication infrastructure required for Phase 1.1.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationInfrastructure(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<AuthenticationState>();
        services.AddSingleton<ISessionManager, SessionManager>();
        services.AddSingleton<IApplicationContext, ApplicationContext>();

        return services;
    }
}
