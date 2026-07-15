namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Request model for updating a vehicle assignment.
/// </summary>
public sealed class UpdateVehicleAssignmentRequest
{
    /// <summary>
    /// Gets or sets the assignment ID.
    /// </summary>
    public int AssignmentId { get; set; }

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

    /// <summary>
    /// Gets or sets the release date (nullable).
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets whether the assignment is active.
    /// </summary>
    public bool IsActive { get; set; }
}
