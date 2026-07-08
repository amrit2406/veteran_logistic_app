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
}
