namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Request model for updating fuel pump status.
/// </summary>
public sealed class UpdateFuelPumpStatusRequest
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int FuelPumpId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
