namespace veteran_logistic.FinancialYear.Models;

/// <summary>
/// Encapsulates the result of a financial year selection operation.
/// </summary>
public sealed class FinancialYearSelectionResult
{
    /// <summary>
    /// Gets whether the selection was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the selected financial year, if successful.
    /// </summary>
    public veteran_logistic.FinancialYear.Models.FinancialYear? SelectedFinancialYear { get; }

    /// <summary>
    /// Gets the error message, if unsuccessful.
    /// </summary>
    public string? ErrorMessage { get; }

    private FinancialYearSelectionResult(bool isSuccess, veteran_logistic.FinancialYear.Models.FinancialYear? selectedFinancialYear, string? errorMessage)
    {
        IsSuccess = isSuccess;
        SelectedFinancialYear = selectedFinancialYear;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static FinancialYearSelectionResult Success(veteran_logistic.FinancialYear.Models.FinancialYear selectedFinancialYear) => 
        new(true, selectedFinancialYear ?? throw new ArgumentNullException(nameof(selectedFinancialYear)), null);

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    public static FinancialYearSelectionResult Failure(string errorMessage) => 
        new(false, null, errorMessage ?? throw new ArgumentNullException(nameof(errorMessage)));
}
