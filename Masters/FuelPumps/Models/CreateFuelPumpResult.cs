namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Result model for fuel pump creation operations.
/// </summary>
public sealed class CreateFuelPumpResult
{
    /// <summary>
    /// Gets or sets the ID of the created fuel pump.
    /// </summary>
    public int FuelPumpId { get; set; }

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
    /// <param name="fuelPumpId">The ID of the created fuel pump.</param>
    /// <returns>A successful result.</returns>
    public static CreateFuelPumpResult Success(int fuelPumpId)
    {
        return new CreateFuelPumpResult
        {
            FuelPumpId = fuelPumpId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreateFuelPumpResult Failure(string errorMessage)
    {
        return new CreateFuelPumpResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
