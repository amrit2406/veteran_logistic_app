namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Result model for vehicle assignment operations.
/// </summary>
public sealed class AssignVehicleResult
{
    /// <summary>
    /// Gets or sets the ID of the created assignment.
    /// </summary>
    public int AssignmentId { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="assignmentId">The ID of the created assignment.</param>
    /// <returns>A successful result.</returns>
    public static AssignVehicleResult Success(int assignmentId)
    {
        return new AssignVehicleResult
        {
            AssignmentId = assignmentId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static AssignVehicleResult Failure(string errorMessage)
    {
        return new AssignVehicleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
