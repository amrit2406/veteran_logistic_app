using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Services;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers Remember Me services for Phase 1.5.
/// </summary>
public static class AuthenticationRememberMeServiceCollectionExtensions
{
    /// <summary>
    /// Adds Remember Me preference persistence services.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationRememberMe(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IRememberMeService, RememberMeService>();

        return services;
    }
}
