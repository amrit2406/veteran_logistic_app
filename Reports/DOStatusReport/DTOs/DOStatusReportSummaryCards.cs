namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents summary cards (KPIs) for the DO status report.
/// </summary>
public sealed class DOStatusReportSummaryCards
{
    /// <summary>
    /// Gets or sets the total DO count.
    /// </summary>
    public int TotalDO { get; set; }

    /// <summary>
    /// Gets or sets the today's loading count.
    /// </summary>
    public int TodayLoading { get; set; }

    /// <summary>
    /// Gets or sets the today's completed count.
    /// </summary>
    public int TodayCompleted { get; set; }

    /// <summary>
    /// Gets or sets the running DO count (loaded but not completed).
    /// </summary>
    public int RunningDO { get; set; }

    /// <summary>
    /// Gets or sets the completed DO count.
    /// </summary>
    public int CompletedDO { get; set; }

    /// <summary>
    /// Gets or sets the payment pending count.
    /// </summary>
    public int PaymentPending { get; set; }

    /// <summary>
    /// Gets or sets the bill pending count.
    /// </summary>
    public int BillPending { get; set; }

    /// <summary>
    /// Gets or sets the delayed DO count.
    /// </summary>
    public int DelayedDO { get; set; }

    /// <summary>
    /// Gets or sets the exception DO count.
    /// </summary>
    public int ExceptionDO { get; set; }

    /// <summary>
    /// Gets or sets the completion percentage.
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// Gets or sets the pending percentage.
    /// </summary>
    public decimal PendingPercentage { get; set; }
}
