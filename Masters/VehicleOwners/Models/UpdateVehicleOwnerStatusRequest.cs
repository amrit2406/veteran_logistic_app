namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Request model for updating vehicle owner status.
/// </summary>
public sealed class UpdateVehicleOwnerStatusRequest
{
    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the active status.
    /// </summary>
    public bool IsActive { get; set; }
}
