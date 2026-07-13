using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents an HSD (High Speed Diesel) rate for a fuel pump on a specific date.
/// </summary>
public class HsdRate : BaseEntity
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump.
    /// </summary>
    public FuelPump? FuelPump { get; set; }

    /// <summary>
    /// Gets or sets the applicable date for this rate.
    /// </summary>
    public DateTime ApplicableDate { get; set; }

    /// <summary>
    /// Gets or sets the rate per litre in decimal(18,2).
    /// </summary>
    public decimal RatePerLitre { get; set; }

    /// <summary>
    /// Gets or sets whether the HSD rate is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the HSD rate has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the HSD rate was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the HSD rate.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the HSD rate.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
