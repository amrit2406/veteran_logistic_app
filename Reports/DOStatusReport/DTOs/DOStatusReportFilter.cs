namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents filter criteria for the DO status report.
/// </summary>
public sealed class DOStatusReportFilter
{
    /// <summary>
    /// Gets or sets the date range start.
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Gets or sets the date range end.
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Gets or sets the customer ID filter.
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle ID filter.
    /// </summary>
    public int? VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the driver name filter.
    /// </summary>
    public string? Driver { get; set; }

    /// <summary>
    /// Gets or sets the material ID filter.
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the source ID filter.
    /// </summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Gets or sets the destination ID filter.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the DO status filter.
    /// </summary>
    public DOStatus? DOStatus { get; set; }

    /// <summary>
    /// Gets or sets the payment status filter.
    /// </summary>
    public string? PaymentStatus { get; set; }

    /// <summary>
    /// Gets or sets the billing status filter.
    /// </summary>
    public string? BillingStatus { get; set; }

    /// <summary>
    /// Determines whether the filter has any active criteria.
    /// </summary>
    public bool HasFilter =>
        DateFrom.HasValue ||
        DateTo.HasValue ||
        CustomerId.HasValue ||
        VehicleId.HasValue ||
        !string.IsNullOrWhiteSpace(Driver) ||
        MaterialId.HasValue ||
        SourceId.HasValue ||
        DestinationId.HasValue ||
        DOStatus.HasValue ||
        !string.IsNullOrWhiteSpace(PaymentStatus) ||
        !string.IsNullOrWhiteSpace(BillingStatus);

    /// <summary>
    /// Clears all filter criteria.
    /// </summary>
    public void Clear()
    {
        DateFrom = null;
        DateTo = null;
        CustomerId = null;
        VehicleId = null;
        Driver = null;
        MaterialId = null;
        SourceId = null;
        DestinationId = null;
        DOStatus = null;
        PaymentStatus = null;
        BillingStatus = null;
    }
}
