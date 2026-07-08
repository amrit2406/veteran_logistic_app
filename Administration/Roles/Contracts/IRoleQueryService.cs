using veteran_logistic.Administration.Roles.Models;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Service contract for querying role data.
/// </summary>
public interface IRoleQueryService
{
    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of role list items.</returns>
    Task<IReadOnlyList<RoleListItem>> GetAllRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches roles by name or description.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of role list items matching the search criteria.</returns>
    Task<IReadOnlyList<RoleListItem>> SearchRolesAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role for editing by role ID.
    /// </summary>
    /// <param name="roleId">The role ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The role edit model, or null if not found.</returns>
    Task<EditRoleModel?> GetRoleForEditAsync(int roleId, CancellationToken cancellationToken = default);
}
