using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents the association between a role and a permission.
/// This is a bridge table for the many-to-many relationship between roles and permissions.
/// </summary>
public class RolePermission : BaseEntity
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Gets or sets whether the permission is granted to the role.
    /// </summary>
    public bool IsGranted { get; set; } = false;

    /// <summary>
    /// Gets or sets the role associated with this permission assignment.
    /// </summary>
    public Role? Role { get; set; }

    /// <summary>
    /// Gets or sets the permission associated with this role assignment.
    /// </summary>
    public Permission? Permission { get; set; }
}
