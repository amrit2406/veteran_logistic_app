namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Request model for updating an existing fuel pump.
/// </summary>
public sealed class UpdateFuelPumpRequest
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the fuel pump is active.
    /// </summary>
    public bool IsActive { get; set; }
}
