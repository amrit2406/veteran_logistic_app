using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Stores runtime session information for the current application instance.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Gets or sets the current user session.
    /// </summary>
    UserSession? CurrentSession { get; set; }
}
