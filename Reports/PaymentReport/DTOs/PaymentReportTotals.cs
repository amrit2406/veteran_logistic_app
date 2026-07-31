namespace veteran_logistic.Reports.PaymentReport.DTOs;

/// <summary>
/// Represents calculated totals for the payment report.
/// </summary>
public sealed class PaymentReportTotals
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
    /// Gets or sets the total driver commission.
    /// </summary>
    public decimal TotalDriverCommission { get; set; }

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
    public decimal TotalSurchargeAmount { get; set; }

    /// <summary>
    /// Gets or sets the total admin charge.
    /// </summary>
    public decimal TotalAdminCharge { get; set; }

    /// <summary>
    /// Gets or sets the total net payment.
    /// </summary>
    public decimal TotalNetPayment { get; set; }
}
