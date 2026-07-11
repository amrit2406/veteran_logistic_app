namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Request model for deleting a fuel pump.
/// </summary>
public sealed class DeleteFuelPumpRequest
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }
}
