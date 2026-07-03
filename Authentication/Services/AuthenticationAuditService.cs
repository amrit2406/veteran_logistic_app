using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Services;

/// <summary>
/// Service for logging authentication audit entries.
/// </summary>
public sealed class AuthenticationAuditService : IAuthenticationAuditService
{
    private readonly IAuthenticationRepository _authenticationRepository;

    /// <summary>
    /// Initializes a new instance of the AuthenticationAuditService.
    /// </summary>
    /// <param name="authenticationRepository">The authentication repository.</param>
    public AuthenticationAuditService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
    }

    /// <summary>
    /// Logs an authentication attempt asynchronously.
    /// </summary>
    /// <param name="auditEntry">The audit entry to log.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task LogAuthenticationAttemptAsync(AuthenticationAuditEntry auditEntry, CancellationToken cancellationToken = default)
    {
        if (auditEntry is null)
        {
            throw new ArgumentNullException(nameof(auditEntry));
        }

        // Delegate to the repository for persistence
        await _authenticationRepository.RecordAuditEntryAsync(auditEntry, cancellationToken);
    }
}
