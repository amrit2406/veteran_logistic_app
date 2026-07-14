namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Request model for deleting a vehicle.
/// </summary>
public sealed class DeleteVehicleRequest
{
    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int VehicleId { get; set; }
}
