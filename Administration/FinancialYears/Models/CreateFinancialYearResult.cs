namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Result model for financial year creation operations.
/// </summary>
public sealed class CreateFinancialYearResult
{
    /// <summary>
    /// Gets or sets the ID of the created financial year.
    /// </summary>
    public int FinancialYearId { get; set; }

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
    /// <param name="financialYearId">The ID of the created financial year.</param>
    /// <returns>A successful result.</returns>
    public static CreateFinancialYearResult Success(int financialYearId)
    {
        return new CreateFinancialYearResult
        {
            FinancialYearId = financialYearId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateFinancialYearResult Failure(string errorMessage)
    {
        return new CreateFinancialYearResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
