namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Result model for vehicle creation operations.
/// </summary>
public sealed class CreateVehicleResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the ID of the created vehicle.
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="vehicleId">The ID of the created vehicle.</param>
    /// <returns>A successful result.</returns>
    public static CreateVehicleResult Success(int vehicleId)
    {
        return new CreateVehicleResult
        {
            IsSuccess = true,
            VehicleId = vehicleId
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateVehicleResult Failure(string errorMessage)
    {
        return new CreateVehicleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
