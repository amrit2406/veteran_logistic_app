namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Result model for vehicle assignment update operations.
/// </summary>
public sealed class UpdateVehicleAssignmentResult
{
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
    /// <returns>A successful result.</returns>
    public static UpdateVehicleAssignmentResult Success()
    {
        return new UpdateVehicleAssignmentResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateVehicleAssignmentResult Failure(string errorMessage)
    {
        return new UpdateVehicleAssignmentResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
