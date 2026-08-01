namespace veteran_logistic.Reports.TdsReport.DTOs;

/// <summary>
/// Represents calculated totals for the TDS report.
/// </summary>
public sealed class TdsReportTotals
{
    /// <summary>
    /// Gets or sets the total record count.
    /// </summary>
    public int RecordCount { get; set; }

    /// <summary>
    /// Gets or sets the total challan amount.
    /// </summary>
    public decimal TotalChallanAmount { get; set; }

    /// <summary>
    /// Gets or sets the total TDS amount.
    /// </summary>
    public decimal TotalTDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the total surcharge amount.
    /// </summary>
    public decimal TotalSurcharge { get; set; }

    /// <summary>
    /// Gets or sets the total admin charge.
    /// </summary>
    public decimal TotalAdminCharge { get; set; }

    /// <summary>
    /// Gets or sets the total net payment.
    /// </summary>
    public decimal TotalNetPayment { get; set; }

    /// <summary>
    /// Gets or sets the average TDS amount.
    /// </summary>
    public decimal AverageTDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the highest TDS amount.
    /// </summary>
    public decimal HighestTDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the lowest TDS amount.
    /// </summary>
    public decimal LowestTDSAmount { get; set; }
}
