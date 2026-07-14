namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Request model for updating vehicle status.
/// </summary>
public sealed class UpdateVehicleStatusRequest
{
    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the active status.
    /// </summary>
    public bool IsActive { get; set; }
}
