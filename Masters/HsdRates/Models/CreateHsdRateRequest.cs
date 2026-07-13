namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Request model for creating a new HSD rate.
/// </summary>
public sealed class CreateHsdRateRequest
{
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
    public bool IsActive { get; set; } = true;
}
