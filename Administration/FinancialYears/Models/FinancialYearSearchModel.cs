namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Represents a search model for filtering financial years.
/// </summary>
public sealed class FinancialYearSearchModel
{
    /// <summary>
    /// Gets or sets the search text to filter financial years by name.
    /// </summary>
    public string SearchText { get; set; } = string.Empty;
}
