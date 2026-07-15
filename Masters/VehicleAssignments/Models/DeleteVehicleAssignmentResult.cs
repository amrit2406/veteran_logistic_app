namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Result model for vehicle assignment delete operations.
/// </summary>
public sealed class DeleteVehicleAssignmentResult
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
    public static DeleteVehicleAssignmentResult Success()
    {
        return new DeleteVehicleAssignmentResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteVehicleAssignmentResult Failure(string errorMessage)
    {
        return new DeleteVehicleAssignmentResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
