using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.ViewModels;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers authentication UI services for Phase 1.3.
/// </summary>
public static class AuthenticationUIServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authentication UI infrastructure required for Phase 1.3.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationUI(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        // Register LoginViewModel as transient
        services.AddTransient<LoginViewModel>();

        return services;
    }
}
