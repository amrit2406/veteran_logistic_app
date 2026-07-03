using veteran_logistic.Authentication.Models;
using VeteranLogistics.Data.Entities.Administration;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for authentication data access operations.
/// </summary>
public interface IAuthenticationRepository
{
    /// <summary>
    /// Retrieves a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found, otherwise null.</returns>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user with Id populated.</returns>
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Records an authentication audit entry asynchronously.
    /// </summary>
    /// <param name="auditEntry">The audit entry to record.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RecordAuditEntryAsync(AuthenticationAuditEntry auditEntry, CancellationToken cancellationToken = default);
}
