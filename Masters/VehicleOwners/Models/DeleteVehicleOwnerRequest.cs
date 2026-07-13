namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Request model for deleting a vehicle owner.
/// </summary>
public sealed class DeleteVehicleOwnerRequest
{
    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int VehicleOwnerId { get; set; }
}
