using veteran_logistic.Administration.Permissions.Models;

namespace veteran_logistic.Administration.Permissions.Contracts;

/// <summary>
/// Service contract for querying permission data.
/// </summary>
public interface IPermissionQueryService
{
    /// <summary>
    /// Gets the complete permission matrix with roles and permissions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The permission matrix model containing roles and permissions.</returns>
    Task<PermissionMatrixModel> GetPermissionMatrixAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all permissions.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of permission list items.</returns>
    Task<IReadOnlyList<PermissionListItem>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
}
