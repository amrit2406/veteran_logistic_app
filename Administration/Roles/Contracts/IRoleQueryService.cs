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
}
