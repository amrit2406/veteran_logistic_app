namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Request model for updating an existing HSD rate.
/// </summary>
public sealed class UpdateHsdRateRequest
{
    /// <summary>
    /// Gets or sets the HSD rate ID.
    /// </summary>
    public int HsdRateId { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }

    /// <summary>
    /// Gets or sets the applicable date.
    /// </summary>
    public DateTime ApplicableDate { get; set; }

    /// <summary>
    /// Gets or sets the rate per litre.
    /// </summary>
    public decimal RatePerLitre { get; set; }

    /// <summary>
    /// Gets or sets whether the HSD rate is active.
    /// </summary>
    public bool IsActive { get; set; }
}
