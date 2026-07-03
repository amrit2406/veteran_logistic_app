using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for application authentication services.
/// Behavior will be implemented in a later phase.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Gets the current authentication state for the running application.
    /// </summary>
    AuthenticationState AuthenticationState { get; }
}
