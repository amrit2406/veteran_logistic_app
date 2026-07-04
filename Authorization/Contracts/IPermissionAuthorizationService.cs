using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Contracts;

/// <summary>
/// Provides permission-based authorization services based on the current authenticated user's role.
/// </summary>
public interface IPermissionAuthorizationService
{
    /// <summary>
    /// Determines whether the current authenticated user has the specified permission.
    /// </summary>
    /// <param name="permission">The permission to check.</param>
    /// <returns>True if the user has the specified permission; otherwise, false.</returns>
    bool HasPermission(ApplicationPermission permission);

    /// <summary>
    /// Determines whether the current authenticated user has any of the specified permissions.
    /// </summary>
    /// <param name="permissions">The permissions to check.</param>
    /// <returns>True if the user has any of the specified permissions; otherwise, false.</returns>
    bool HasAnyPermission(params ApplicationPermission[] permissions);

    /// <summary>
    /// Determines whether the current authenticated user has all of the specified permissions.
    /// </summary>
    /// <param name="permissions">The permissions to check.</param>
    /// <returns>True if the user has all of the specified permissions; otherwise, false.</returns>
    bool HasAllPermissions(params ApplicationPermission[] permissions);
}
