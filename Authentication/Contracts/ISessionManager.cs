using veteran_logistic.Authentication.Events;
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

    /// <summary>
    /// Gets a value indicating whether there is an active session.
    /// </summary>
    bool HasActiveSession { get; }

    /// <summary>
    /// Gets the session identifier.
    /// </summary>
    string? SessionId { get; }

    /// <summary>
    /// Gets the time the session started.
    /// </summary>
    DateTimeOffset LoginTime { get; }

    /// <summary>
    /// Gets the last time the session was updated.
    /// </summary>
    DateTimeOffset LastActivity { get; }

    /// <summary>
    /// Occurs when the session changes.
    /// </summary>
    event EventHandler<SessionChangedEventArgs>? SessionChanged;

    /// <summary>
    /// Determines whether the current session is valid.
    /// </summary>
    /// <returns>True if the session is valid; otherwise, false.</returns>
    bool IsSessionValid();

    /// <summary>
    /// Updates the last activity timestamp for the current session.
    /// </summary>
    void UpdateActivity();

    /// <summary>
    /// Clears the current session.
    /// </summary>
    void ClearSession();
}
