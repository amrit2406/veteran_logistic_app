using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a vehicle assignment in the system.
/// Records the ownership history of vehicles over time.
/// </summary>
public class VehicleAssignment : BaseEntity
{
    /// <summary>
    /// Gets or sets the vehicle ID (foreign key).
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle owner ID (foreign key).
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the date when the vehicle was assigned to the owner.
    /// </summary>
    public DateTime AssignDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the vehicle was released from the owner (nullable).
    /// When null, the vehicle is currently assigned to this owner.
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Navigation property to the vehicle.
    /// </summary>
    public Vehicle? Vehicle { get; set; }

    /// <summary>
    /// Navigation property to the vehicle owner.
    /// </summary>
    public VehicleOwner? VehicleOwner { get; set; }

    /// <summary>
    /// Gets or sets whether the vehicle assignment is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the vehicle assignment has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the vehicle assignment was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the vehicle assignment.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the vehicle assignment.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
