namespace veteran_logistic.Reports.UnloadingReport.DTOs;

/// <summary>
/// Represents calculated totals for the unloading report.
/// </summary>
public sealed class UnloadingReportTotals
{
    /// <summary>
    /// Gets or sets the total record count.
    /// </summary>
    public int RecordCount { get; set; }

    /// <summary>
    /// Gets or sets the total gross weight.
    /// </summary>
    public decimal TotalGrossWeight { get; set; }

    /// <summary>
    /// Gets or sets the total tare weight.
    /// </summary>
    public decimal TotalTareWeight { get; set; }

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
    /// Gets or sets the total fuel amount.
    /// </summary>
    public decimal TotalFuelAmount { get; set; }

    /// <summary>
    /// Gets or sets the total cash advance.
    /// </summary>
    public decimal TotalCashAdvance { get; set; }

    /// <summary>
    /// Gets or sets the total other advance.
    /// </summary>
    public decimal TotalOtherAdvance { get; set; }
}
