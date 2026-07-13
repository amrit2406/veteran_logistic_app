namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Result model for vehicle owner delete operations.
/// </summary>
public sealed class DeleteVehicleOwnerResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
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
    public static DeleteVehicleOwnerResult Success()
    {
        return new DeleteVehicleOwnerResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteVehicleOwnerResult Failure(string errorMessage)
    {
        return new DeleteVehicleOwnerResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
