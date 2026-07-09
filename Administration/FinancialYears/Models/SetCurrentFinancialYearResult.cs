namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Result model for set current financial year operations.
/// </summary>
public sealed class SetCurrentFinancialYearResult
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
    public static SetCurrentFinancialYearResult Success()
    {
        return new SetCurrentFinancialYearResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static SetCurrentFinancialYearResult Failure(string errorMessage)
    {
        return new SetCurrentFinancialYearResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
