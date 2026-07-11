namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Request model for creating a new fuel pump.
/// </summary>
public sealed class CreateFuelPumpRequest
{
    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the fuel pump is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
