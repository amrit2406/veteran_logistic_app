namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Result model for HSD rate creation operations.
/// </summary>
public sealed class CreateHsdRateResult
{
    /// <summary>
    /// Gets or sets the ID of the created HSD rate.
    /// </summary>
    public int HsdRateId { get; set; }

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
    /// <param name="hsdRateId">The ID of the created HSD rate.</param>
    /// <returns>A successful result.</returns>
    public static CreateHsdRateResult Success(int hsdRateId)
    {
        return new CreateHsdRateResult
        {
            HsdRateId = hsdRateId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreateHsdRateResult Failure(string errorMessage)
    {
        return new CreateHsdRateResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
