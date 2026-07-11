using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a fuel pump in the system.
/// </summary>
public class FuelPump : BaseEntity
{
    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the fuel pump is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the fuel pump has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the fuel pump was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the fuel pump.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the fuel pump.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
