using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Represents the current application context for runtime access.
/// </summary>
public interface IApplicationContext
{
    /// <summary>
    /// Gets or sets the current authenticated user for the application.
    /// </summary>
    AuthenticatedUser? CurrentUser { get; set; }

    /// <summary>
    /// Gets or sets the current authentication state.
    /// </summary>
    AuthenticationState AuthenticationState { get; set; }
}
