namespace veteran_logistic.Administration.Permissions.Models;

/// <summary>
/// Represents the complete permission matrix model containing roles and permissions.
/// </summary>
public sealed class PermissionMatrixModel
{
    /// <summary>
    /// Gets or sets the collection of roles in the system.
    /// </summary>
    public List<RoleMatrixItem> Roles { get; set; } = new();

    /// <summary>
    /// Gets or sets the collection of permission matrix rows.
    /// </summary>
    public List<PermissionMatrixRow> Permissions { get; set; } = new();
}

/// <summary>
/// Represents a role item in the permission matrix.
/// </summary>
public sealed class RoleMatrixItem
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of granted permission IDs for this role.
    /// </summary>
    public HashSet<int> GrantedPermissionIds { get; set; } = new();
}
