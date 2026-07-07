namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Represents a role item for display in the role listing grid.
/// </summary>
public sealed class RoleListItem
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether the role is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets whether this is a system role.
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// Gets or sets the date the role was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }
}
