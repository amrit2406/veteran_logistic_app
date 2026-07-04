using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Services;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers logout services for Phase 1.10.
/// </summary>
public static class AuthenticationLogoutServiceCollectionExtensions
{
    /// <summary>
    /// Adds the logout service.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationLogout(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<ILogoutService, LogoutService>();

        return services;
    }
}
