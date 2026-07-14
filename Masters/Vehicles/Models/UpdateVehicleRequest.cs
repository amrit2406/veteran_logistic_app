namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Request model for updating a vehicle.
/// </summary>
public sealed class UpdateVehicleRequest
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
    /// Gets or sets the vehicle number.
    /// </summary>
    public string VehicleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vehicle is active.
    /// </summary>
    public bool IsActive { get; set; }
}
