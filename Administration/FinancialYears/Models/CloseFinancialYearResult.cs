namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Result model for close financial year operations.
/// </summary>
public sealed class CloseFinancialYearResult
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
    public static CloseFinancialYearResult Success()
    {
        return new CloseFinancialYearResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CloseFinancialYearResult Failure(string errorMessage)
    {
        return new CloseFinancialYearResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
