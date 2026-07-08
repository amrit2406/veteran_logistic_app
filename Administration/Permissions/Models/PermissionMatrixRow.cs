namespace veteran_logistic.Administration.Permissions.Models;

/// <summary>
/// Represents a row in the permission matrix, showing a permission and its grant status across roles.
/// </summary>
public sealed class PermissionMatrixRow
{
    /// <summary>
    /// Gets or sets the permission ID.
    /// </summary>
    public int PermissionId { get; set; }

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
