namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Stores runtime session information for the current user.
/// </summary>
public sealed class UserSession
{
    /// <summary>
    /// Gets or sets the session identifier.
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier associated with the session.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the username associated with the session.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the time the session started.
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the last time the session was updated.
    /// </summary>
    public DateTimeOffset? LastActivityAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the session is currently active.
    /// </summary>
    public bool IsActive { get; set; }
}
