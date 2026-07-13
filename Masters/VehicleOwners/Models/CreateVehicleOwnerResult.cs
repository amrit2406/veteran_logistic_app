namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Result model for vehicle owner creation operations.
/// </summary>
public sealed class CreateVehicleOwnerResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the ID of the created vehicle owner.
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="vehicleOwnerId">The ID of the created vehicle owner.</param>
    /// <returns>A successful result.</returns>
    public static CreateVehicleOwnerResult Success(int vehicleOwnerId)
    {
        return new CreateVehicleOwnerResult
        {
            IsSuccess = true,
            VehicleOwnerId = vehicleOwnerId
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateVehicleOwnerResult Failure(string errorMessage)
    {
        return new CreateVehicleOwnerResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
