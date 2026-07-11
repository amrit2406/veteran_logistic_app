namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Result model for fuel pump update operations.
/// </summary>
public sealed class UpdateFuelPumpResult
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
    public static UpdateFuelPumpResult Success()
    {
        return new UpdateFuelPumpResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateFuelPumpResult Failure(string errorMessage)
    {
        return new UpdateFuelPumpResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
