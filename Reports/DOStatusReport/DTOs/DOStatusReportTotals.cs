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
}
