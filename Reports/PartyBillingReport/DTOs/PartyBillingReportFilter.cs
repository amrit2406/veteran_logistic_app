namespace veteran_logistic.Reports.PartyBillingReport.DTOs;

/// <summary>
/// Represents filter criteria for the party billing report.
/// </summary>
public sealed class PartyBillingReportFilter
{
    /// <summary>
    /// Gets or sets the bill date range start.
    /// </summary>
    public DateTime? BillDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the bill date range end.
    /// </summary>
    public DateTime? BillDateTo { get; set; }

    /// <summary>
    /// Gets or sets the loading date range start.
    /// </summary>
    public DateTime? LoadingDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the loading date range end.
    /// </summary>
    public DateTime? LoadingDateTo { get; set; }

    /// <summary>
    /// Gets or sets the company ID filter.
    /// </summary>
    public int? CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the customer ID filter.
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the third party name filter.
    /// </summary>
    public string? ThirdParty { get; set; }

    /// <summary>
    /// Gets or sets the permit number filter.
    /// </summary>
    public string? PermitNumber { get; set; }

    /// <summary>
    /// Gets or sets the bill number filter.
    /// </summary>
    public string? BillNumber { get; set; }

    /// <summary>
    /// Gets or sets the vehicle ID filter.
    /// </summary>
    public int? VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the material ID filter.
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the consignor ID filter.
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignee ID filter.
    /// </summary>
    public int? ConsigneeId { get; set; }

    /// <summary>
    /// Gets or sets the destination ID filter.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the status filter.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Determines whether the filter has any active criteria.
    /// </summary>
    public bool HasFilter =>
        BillDateFrom.HasValue ||
        BillDateTo.HasValue ||
        LoadingDateFrom.HasValue ||
        LoadingDateTo.HasValue ||
        CompanyId.HasValue ||
        CustomerId.HasValue ||
        !string.IsNullOrWhiteSpace(ThirdParty) ||
        !string.IsNullOrWhiteSpace(PermitNumber) ||
        !string.IsNullOrWhiteSpace(BillNumber) ||
        VehicleId.HasValue ||
        MaterialId.HasValue ||
        ConsignorId.HasValue ||
        ConsigneeId.HasValue ||
        DestinationId.HasValue ||
        !string.IsNullOrWhiteSpace(Status);

    /// <summary>
    /// Clears all filter criteria.
    /// </summary>
    public void Clear()
    {
        BillDateFrom = null;
        BillDateTo = null;
        LoadingDateFrom = null;
        LoadingDateTo = null;
        CompanyId = null;
        CustomerId = null;
        ThirdParty = null;
        PermitNumber = null;
        BillNumber = null;
        VehicleId = null;
        MaterialId = null;
        ConsignorId = null;
        ConsigneeId = null;
        DestinationId = null;
        Status = null;
    }
}
