using Microsoft.EntityFrameworkCore;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;

namespace veteran_logistic.Authentication.Repositories;

/// <summary>
/// Repository for authentication data access operations.
/// </summary>
public sealed class AuthenticationRepository : IAuthenticationRepository
{
    private readonly VeteranLogisticsDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the AuthenticationRepository.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public AuthenticationRepository(VeteranLogisticsDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Retrieves a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found, otherwise null.</returns>
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user with Id populated.</returns>
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.CreatedOn = DateTime.UtcNow;
        user.ModifiedOn = null;

        await _dbContext.Set<User>().AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    /// <summary>
    /// Records an authentication audit entry asynchronously.
    /// </summary>
    /// <param name="auditEntry">The audit entry to record.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task RecordAuditEntryAsync(AuthenticationAuditEntry auditEntry, CancellationToken cancellationToken = default)
    {
        if (auditEntry is null)
        {
            throw new ArgumentNullException(nameof(auditEntry));
        }

        // Note: AuthenticationAuditEntry persistence will be implemented when the entity is added to the data layer
        // For now, this is a placeholder for the audit logging infrastructure
        await Task.CompletedTask;
    }
}
