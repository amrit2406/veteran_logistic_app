using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a source/destination location in the system.
/// </summary>
public class SourceDestination : BaseEntity
{
    /// <summary>
    /// Gets or sets the source/destination name.
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the source/destination is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the source/destination has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the source/destination was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the source/destination.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the source/destination.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
