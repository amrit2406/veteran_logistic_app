namespace veteran_logistic.Reports.UnloadingReport.DTOs;

/// <summary>
/// Represents filter criteria for the unloading report.
/// </summary>
public sealed class UnloadingReportFilter
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
    /// Gets or sets the company ID filter.
    /// </summary>
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the customer ID filter.
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the consignor ID filter.
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignee ID filter.
    /// </summary>
    public int? ConsigneeId { get; set; }

    /// <summary>
    /// Gets or sets the source ID filter.
    /// </summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Gets or sets the destination ID filter.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle ID filter.
    /// </summary>
    public int? VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the material ID filter.
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the driver name filter.
    /// </summary>
    public string? Driver { get; set; }

    /// <summary>
    /// Gets or sets the owner ID filter.
    /// </summary>
    public int? OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the union/vendor ID filter.
    /// </summary>
    public int? UnionVendorId { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID filter.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the challan number filter.
    /// </summary>
    public string? ChallanNumber { get; set; }

    /// <summary>
    /// Gets or sets the TP number filter.
    /// </summary>
    public string? TPNumber { get; set; }

    /// <summary>
    /// Gets or sets the status filter (active/inactive).
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Determines whether the filter has any active criteria.
    /// </summary>
    public bool HasFilter =>
        DateFrom.HasValue ||
        DateTo.HasValue ||
        CompanyId.HasValue ||
        CustomerId.HasValue ||
        ConsignorId.HasValue ||
        ConsigneeId.HasValue ||
        SourceId.HasValue ||
        DestinationId.HasValue ||
        VehicleId.HasValue ||
        MaterialId.HasValue ||
        !string.IsNullOrWhiteSpace(Driver) ||
        OwnerId.HasValue ||
        UnionVendorId.HasValue ||
        PaymentLocationId.HasValue ||
        !string.IsNullOrWhiteSpace(ChallanNumber) ||
        !string.IsNullOrWhiteSpace(TPNumber) ||
        IsActive.HasValue;

    /// <summary>
    /// Clears all filter criteria.
    /// </summary>
    public void Clear()
    {
        DateFrom = null;
        DateTo = null;
        CompanyId = null;
        CustomerId = null;
        ConsignorId = null;
        ConsigneeId = null;
        SourceId = null;
        DestinationId = null;
        VehicleId = null;
        MaterialId = null;
        Driver = null;
        OwnerId = null;
        UnionVendorId = null;
        PaymentLocationId = null;
        ChallanNumber = null;
        TPNumber = null;
        IsActive = null;
    }
}
