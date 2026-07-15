namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Request model for deleting a vehicle assignment (soft delete).
/// </summary>
public sealed class DeleteVehicleAssignmentRequest
{
    /// <summary>
    /// Gets or sets the assignment ID.
    /// </summary>
    public int AssignmentId { get; set; }
}
