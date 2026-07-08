namespace veteran_logistic.Administration.Permissions.Models;

/// <summary>
/// Request model for assigning permissions to a role.
/// </summary>
public sealed class AssignPermissionsRequest
{
    /// <summary>
    /// Gets or sets the role ID to assign permissions to.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the collection of permission assignments.
    /// </summary>
    public ICollection<PermissionAssignmentItem> PermissionAssignments { get; set; } = new List<PermissionAssignmentItem>();
}

/// <summary>
/// Represents a single permission assignment item.
/// </summary>
public sealed class PermissionAssignmentItem
{
    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    public int PermissionId { get; set; }

    /// <summary>
    /// Gets or sets whether the permission is granted to the role.
    /// </summary>
    public bool IsGranted { get; set; }
}
