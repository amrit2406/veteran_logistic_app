namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Data transfer object for displaying HSD rate information in a list.
/// </summary>
public sealed class HsdRateListItem
{
    /// <summary>
    /// Gets or sets the HSD rate ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName { get; set; } = string.Empty;

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
