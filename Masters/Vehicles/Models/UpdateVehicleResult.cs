namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Result model for vehicle update operations.
/// </summary>
public sealed class UpdateVehicleResult
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
    public static UpdateVehicleResult Success()
    {
        return new UpdateVehicleResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateVehicleResult Failure(string errorMessage)
    {
        return new UpdateVehicleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
