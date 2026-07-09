namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Request model for setting a financial year as current.
/// </summary>
public sealed class SetCurrentFinancialYearRequest
{
    /// <summary>
    /// Gets or sets the financial year ID.
    /// </summary>
    public int FinancialYearId { get; set; }
}
