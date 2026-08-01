namespace veteran_logistic.Reports.PartyBillingReport.DTOs;

/// <summary>
/// Represents calculated totals for the party billing report.
/// </summary>
public sealed class PartyBillingReportTotals
{
    /// <summary>
    /// Gets or sets the total record count.
    /// </summary>
    public int RecordCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of bills.
    /// </summary>
    public int TotalBills { get; set; }

    /// <summary>
    /// Gets or sets the total number of challans.
    /// </summary>
    public int TotalChallans { get; set; }

    /// <summary>
    /// Gets or sets the total loading weight.
    /// </summary>
    public decimal TotalLoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the total gross amount.
    /// </summary>
    public decimal TotalGrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the average bill amount.
    /// </summary>
    public decimal AverageBillAmount { get; set; }
}
