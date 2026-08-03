namespace veteran_logistic.Reports.ConsolidatedReport.DTOs;

/// <summary>
/// Represents calculated totals for the consolidated report.
/// </summary>
public sealed class ConsolidatedReportTotals
{
    /// <summary>
    /// Gets or sets the total record count.
    /// </summary>
    public int RecordCount { get; set; }

    /// <summary>
    /// Gets or sets the total loading weight.
    /// </summary>
    public decimal TotalLoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the total unloading weight.
    /// </summary>
    public decimal TotalUnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the total shortage weight.
    /// </summary>
    public decimal TotalShortageWeight { get; set; }

    /// <summary>
    /// Gets or sets the total loading amount.
    /// </summary>
    public decimal TotalLoadingAmount { get; set; }

    /// <summary>
    /// Gets or sets the total challan amount.
    /// </summary>
    public decimal TotalChallanAmount { get; set; }

    /// <summary>
    /// Gets or sets the total net payment.
    /// </summary>
    public decimal TotalNetPayment { get; set; }

    /// <summary>
    /// Gets or sets the total TDS amount.
    /// </summary>
    public decimal TotalTDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the total bills count.
    /// </summary>
    public int TotalBills { get; set; }

    /// <summary>
    /// Gets or sets the average net payment.
    /// </summary>
    public decimal AverageNetPayment { get; set; }
}
