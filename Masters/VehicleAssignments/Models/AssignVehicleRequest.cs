namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Request model for assigning a vehicle to an owner.
/// </summary>
public sealed class AssignVehicleRequest
{
    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the assign date.
    /// </summary>
    public DateTime AssignDate { get; set; }
}
