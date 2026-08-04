namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents calculated totals for the DO status report.
/// </summary>
public sealed class DOStatusReportTotals
{
    /// <summary>
    /// Gets or sets the total records count.
    /// </summary>
    public int TotalRecords { get; set; }

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
    /// Gets or sets the total gross amount.
    /// </summary>
    public decimal TotalGrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the total challan money.
    /// </summary>
    public decimal TotalChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the total pending amount.
    /// </summary>
    public decimal TotalPendingAmount { get; set; }

    /// <summary>
    /// Gets or sets the completed gross amount.
    /// </summary>
    public decimal CompletedGrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the pending gross amount.
    /// </summary>
    public decimal PendingGrossAmount { get; set; }

    /// <summary>
    /// Gets or sets today's gross amount.
    /// </summary>
    public decimal TodayGrossAmount { get; set; }

    /// <summary>
    /// Gets or sets today's loading weight.
    /// </summary>
    public decimal TodayLoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the completed loading weight.
    /// </summary>
    public decimal CompletedLoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the pending loading weight.
    /// </summary>
    public decimal PendingLoadingWeight { get; set; }
}
