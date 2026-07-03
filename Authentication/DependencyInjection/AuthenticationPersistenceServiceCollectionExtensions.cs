using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Repositories;
using veteran_logistic.Authentication.Security;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers authentication persistence and security services for Phase 1.2.
/// </summary>
public static class AuthenticationPersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authentication persistence and security infrastructure required for Phase 1.2.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationPersistence(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        // Register repository
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

        // Register security services
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPasswordPolicy, PasswordPolicy>();

        return services;
    }
}
