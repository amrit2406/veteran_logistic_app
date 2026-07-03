using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for application authentication services.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Gets the current authentication state for the running application.
    /// </summary>
    AuthenticationState AuthenticationState { get; }

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether authentication was successful.</returns>
    Task<AuthenticationResult> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
