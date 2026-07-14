using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a vehicle in the system.
/// </summary>
public class Vehicle : BaseEntity
{
    /// <summary>
    /// Gets or sets the vehicle owner ID (foreign key).
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle number (required, unique, max length 30).
    /// </summary>
    public string VehicleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle type (10 Wheels, 12 Wheels, 14 Wheels, 16 Wheels, 18 Wheels, 20 Wheels, 22 Wheels).
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the vehicle owner.
    /// </summary>
    public VehicleOwner? VehicleOwner { get; set; }

    /// <summary>
    /// Gets or sets whether the vehicle is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the vehicle has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the vehicle was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the vehicle.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the vehicle.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
