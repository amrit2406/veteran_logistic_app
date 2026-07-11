using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a material in the system.
/// </summary>
public class Material : BaseEntity
{
    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the material is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the material has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the material was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the material.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the material.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
