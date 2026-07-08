namespace veteran_logistic.Administration.Permissions.Models;

/// <summary>
/// Represents a permission item for display in the permission matrix.
/// </summary>
public sealed class PermissionListItem
{
    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the module this permission belongs to.
    /// </summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the screen this permission applies to.
    /// </summary>
    public string Screen { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique permission key.
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
}
