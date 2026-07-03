using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Services;

namespace veteran_logistic.Authentication.DependencyInjection;

/// <summary>
/// Registers authentication workflow services for Phase 1.4.
/// </summary>
public static class AuthenticationWorkflowServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authentication workflow services.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddAuthenticationWorkflow(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IAuthenticationAuditService, AuthenticationAuditService>();

        return services;
    }
}
