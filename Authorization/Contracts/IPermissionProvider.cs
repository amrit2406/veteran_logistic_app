using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Contracts;

/// <summary>
/// Provides permission mapping for application roles.
/// </summary>
public interface IPermissionProvider
{
    /// <summary>
    /// Gets the permissions associated with the specified role name.
    /// </summary>
    /// <param name="roleName">The role name.</param>
    /// <returns>A collection of permissions for the role.</returns>
    IEnumerable<ApplicationPermission> GetPermissions(string roleName);
}
