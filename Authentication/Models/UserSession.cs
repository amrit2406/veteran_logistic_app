namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Stores runtime session information for the current user.
/// </summary>
public sealed class UserSession
{
    private DateTimeOffset _lastActivity;
    private DateTimeOffset? _lastActivityAt;

    /// <summary>
    /// Gets the session identifier.
    /// </summary>
    public string? SessionId { get; init; }

    /// <summary>
    /// Gets the time the session started.
    /// </summary>
    public DateTimeOffset StartedAt { get; init; }

    /// <summary>
    /// Gets the last time the session was updated.
    /// </summary>
    public DateTimeOffset? LastActivityAt
    {
        get => _lastActivityAt;
        init => _lastActivityAt = value;
    }

    /// <summary>
    /// Gets a value indicating whether the session is currently active.
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Gets the login time for the session.
    /// </summary>
    public DateTimeOffset LoginTime { get; init; }

    /// <summary>
    /// Gets the last activity time for the session.
    /// </summary>
    public DateTimeOffset LastActivity
    {
        get => _lastActivity;
        init => _lastActivity = value;
    }

    /// <summary>
    /// Gets the authenticated user associated with the session.
    /// </summary>
    public AuthenticatedUser? AuthenticatedUser { get; init; }

    /// <summary>
    /// Gets the financial year associated with the session.
    /// </summary>
    public FinancialYear.Models.FinancialYear? FinancialYear { get; init; }

    /// <summary>
    /// Updates the last activity timestamp for the session.
    /// </summary>
    public void UpdateLastActivity()
    {
        _lastActivity = DateTimeOffset.UtcNow;
        _lastActivityAt = DateTimeOffset.UtcNow;
    }
}
