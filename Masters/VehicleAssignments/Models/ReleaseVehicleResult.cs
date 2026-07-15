namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Result model for vehicle release operations.
/// </summary>
public sealed class ReleaseVehicleResult
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
    public static ReleaseVehicleResult Success()
    {
        return new ReleaseVehicleResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static ReleaseVehicleResult Failure(string errorMessage)
    {
        return new ReleaseVehicleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
