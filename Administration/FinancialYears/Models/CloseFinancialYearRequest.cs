namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Request model for closing a financial year.
/// </summary>
public sealed class CloseFinancialYearRequest
{
    /// <summary>
    /// Gets or sets the financial year ID.
    /// </summary>
    public int FinancialYearId { get; set; }
}
