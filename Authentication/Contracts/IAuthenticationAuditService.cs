using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for authentication audit logging.
/// </summary>
public interface IAuthenticationAuditService
{
    /// <summary>
    /// Logs an authentication attempt asynchronously.
    /// </summary>
    /// <param name="auditEntry">The audit entry to log.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task LogAuthenticationAttemptAsync(AuthenticationAuditEntry auditEntry, CancellationToken cancellationToken = default);
}
