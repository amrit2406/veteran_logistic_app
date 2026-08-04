namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents a DO status report item for display in the DO status report grid.
/// </summary>
public sealed class DOStatusReportItem
{
    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the consignor name.
    /// </summary>
    public string? ConsignorName { get; set; }

    /// <summary>
    /// Gets or sets the consignee name.
    /// </summary>
    public string? ConsigneeName { get; set; }

    /// <summary>
    /// Gets or sets the source name.
    /// </summary>
    public string? SourceName { get; set; }

    /// <summary>
    /// Gets or sets the destination name.
    /// </summary>
    public string? DestinationName { get; set; }

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight.
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the shortage weight.
    /// </summary>
    public decimal ShortageWeight { get; set; }

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the pending amount.
    /// </summary>
    public decimal PendingAmount { get; set; }

    /// <summary>
    /// Gets or sets the bill number.
    /// </summary>
    public string? BillNumber { get; set; }

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime? BillDate { get; set; }

    /// <summary>
    /// Gets or sets the DO status.
    /// </summary>
    public DOStatus DOStatus { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public PaymentStatusType PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the billing status.
    /// </summary>
    public BillingStatusType BillingStatus { get; set; }

    /// <summary>
    /// Gets or sets the exception type detected for this DO.
    /// </summary>
    public DOExceptionType ExceptionType { get; set; }

    /// <summary>
    /// Gets or sets the age of the DO in days since loading.
    /// </summary>
    public int AgeInDays { get; set; }

    /// <summary>
    /// Gets or sets whether this DO is delayed beyond the configured threshold.
    /// </summary>
    public bool IsDelayed { get; set; }

    /// <summary>
    /// Gets or sets the delay in days beyond the threshold.
    /// </summary>
    public int DelayDays { get; set; }
}
