namespace veteran_logistic.Authorization.Models;

/// <summary>
/// Defines the application roles for role-based authorization.
/// </summary>
public enum ApplicationRole
{
    /// <summary>
    /// No role assigned.
    /// </summary>
    None,

    /// <summary>
    /// Administrator role with full system access.
    /// </summary>
    Administrator,

    /// <summary>
    /// Manager role with operational oversight.
    /// </summary>
    Manager,

    /// <summary>
    /// User role with standard access.
    /// </summary>
    User,

    /// <summary>
    /// Viewer role with read-only access.
    /// </summary>
    Viewer
}
