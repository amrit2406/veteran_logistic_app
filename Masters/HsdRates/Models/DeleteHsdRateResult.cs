namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Result model for HSD rate deletion operations.
/// </summary>
public sealed class DeleteHsdRateResult
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
    public static DeleteHsdRateResult Success()
    {
        return new DeleteHsdRateResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static DeleteHsdRateResult Failure(string errorMessage)
    {
        return new DeleteHsdRateResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
