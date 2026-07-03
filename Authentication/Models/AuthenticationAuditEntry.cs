namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Represents an authentication audit entry for security logging.
/// </summary>
public sealed class AuthenticationAuditEntry
{
    /// <summary>
    /// Gets or sets the username involved in the authentication attempt.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp of the authentication attempt.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the IP address from which the attempt originated.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets whether the authentication attempt was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the failure reason if authentication failed.
    /// </summary>
    public string? FailureReason { get; set; }
}
