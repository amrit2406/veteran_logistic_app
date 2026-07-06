using veteran_logistic.Administration.Users.Models;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Service contract for querying user data.
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of user list items.</returns>
    Task<IReadOnlyList<UserListItem>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches users by username, display name, or role name.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of user list items matching the search criteria.</returns>
    Task<IReadOnlyList<UserListItem>> SearchUsersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user for editing by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user edit model, or null if not found.</returns>
    Task<EditUserModel?> GetUserForEditAsync(int userId, CancellationToken cancellationToken = default);
}
