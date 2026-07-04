namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Provides read-only session metadata for external consumers.
/// </summary>
public sealed class SessionInfo
{
    /// <summary>
    /// Gets the session identifier.
    /// </summary>
    public string? SessionId { get; init; }

    /// <summary>
    /// Gets the time the session started.
    /// </summary>
    public DateTimeOffset LoginTime { get; init; }

    /// <summary>
    /// Gets the last time the session was updated.
    /// </summary>
    public DateTimeOffset LastActivity { get; init; }

    /// <summary>
    /// Gets the authenticated user associated with the session.
    /// </summary>
    public AuthenticatedUser? AuthenticatedUser { get; init; }

    /// <summary>
    /// Gets the financial year associated with the session.
    /// </summary>
    public FinancialYear.Models.FinancialYear? FinancialYear { get; init; }
}
