namespace veteran_logistic.Reports.ConsolidatedReport.DTOs;

/// <summary>
/// Represents summary cards (KPIs) for the consolidated report.
/// </summary>
public sealed class ConsolidatedReportSummaryCards
{
    /// <summary>
    /// Gets or sets the total transactions count.
    /// </summary>
    public int TotalTransactions { get; set; }

    /// <summary>
    /// Gets or sets the loading only transactions count.
    /// </summary>
    public int LoadingOnly { get; set; }

    /// <summary>
    /// Gets or sets the pending unloading transactions count.
    /// </summary>
    public int PendingUnloading { get; set; }

    /// <summary>
    /// Gets or sets the pending payment transactions count.
    /// </summary>
    public int PendingPayment { get; set; }

    /// <summary>
    /// Gets or sets the pending billing transactions count.
    /// </summary>
    public int PendingBilling { get; set; }

    /// <summary>
    /// Gets or sets the completed transactions count.
    /// </summary>
    public int Completed { get; set; }

    /// <summary>
    /// Gets or sets the total revenue (loading amount).
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Gets or sets the total net payment.
    /// </summary>
    public decimal TotalNetPayment { get; set; }

    /// <summary>
    /// Gets or sets the total TDS amount.
    /// </summary>
    public decimal TotalTDS { get; set; }
}
