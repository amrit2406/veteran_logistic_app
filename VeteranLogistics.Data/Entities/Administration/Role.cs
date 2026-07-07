using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a role in the system.
/// </summary>
public class Role : BaseEntity
{
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
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this is a system role that cannot be deleted.
    /// </summary>
    public bool IsSystemRole { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the role has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the role was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }
}
