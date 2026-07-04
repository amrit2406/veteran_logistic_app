using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Contracts;

/// <summary>
/// Provides role-based authorization services based on the current authenticated user.
/// </summary>
public interface IRoleAuthorizationService
{
    /// <summary>
    /// Determines whether the current authenticated user has the specified role.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <returns>True if the user has the specified role; otherwise, false.</returns>
    bool HasRole(ApplicationRole role);

    /// <summary>
    /// Determines whether the current authenticated user has any of the specified roles.
    /// </summary>
    /// <param name="roles">The roles to check.</param>
    /// <returns>True if the user has any of the specified roles; otherwise, false.</returns>
    bool HasAnyRole(params ApplicationRole[] roles);

    /// <summary>
    /// Determines whether the current authenticated user has all of the specified roles.
    /// </summary>
    /// <param name="roles">The roles to check.</param>
    /// <returns>True if the user has all of the specified roles; otherwise, false.</returns>
    bool HasAllRoles(params ApplicationRole[] roles);

    /// <summary>
    /// Determines whether the current authenticated user is an administrator.
    /// </summary>
    /// <returns>True if the user is an administrator; otherwise, false.</returns>
    bool IsAdministrator();
}
