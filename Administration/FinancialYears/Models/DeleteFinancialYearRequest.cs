namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Request model for deleting a financial year.
/// </summary>
public sealed class DeleteFinancialYearRequest
{
    /// <summary>
    /// Gets or sets the financial year ID.
    /// </summary>
    public int FinancialYearId { get; set; }
}
