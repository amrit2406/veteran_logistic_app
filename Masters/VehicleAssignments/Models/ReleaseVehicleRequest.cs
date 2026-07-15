namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Request model for releasing a vehicle from an owner.
/// </summary>
public sealed class ReleaseVehicleRequest
{
    /// <summary>
    /// Gets or sets the assignment ID.
    /// </summary>
    public int AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets the release date.
    /// </summary>
    public DateTime ReleaseDate { get; set; }
}
