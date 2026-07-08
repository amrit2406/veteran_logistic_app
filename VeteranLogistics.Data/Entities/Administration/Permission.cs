using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a permission in the system.
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Gets or sets the module this permission belongs to (e.g., "Administration", "FinancialYear").
    /// </summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the screen this permission applies to (e.g., "Users", "Roles").
    /// </summary>
    public string Screen { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique permission key (e.g., "User.View", "Role.Create").
    /// </summary>
    public string PermissionKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for the permission.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of what this permission allows.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the sort order for display purposes.
    /// </summary>
    public int SortOrder { get; set; }

    /// <summary>
    /// Gets or sets whether the permission is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the permission has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the permission was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the collection of role permissions for this permission.
    /// </summary>
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
