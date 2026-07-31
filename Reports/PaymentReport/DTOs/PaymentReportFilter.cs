namespace veteran_logistic.Reports.PaymentReport.DTOs;

/// <summary>
/// Represents filter criteria for the payment report.
/// </summary>
public sealed class PaymentReportFilter
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
    /// Gets or sets the vehicle owner ID filter.
    /// </summary>
    public int? OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID filter.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the payment type filter.
    /// </summary>
    public string? PaymentType { get; set; }

    /// <summary>
    /// Gets or sets the beneficiary filter.
    /// </summary>
    public string? Beneficiary { get; set; }

    /// <summary>
    /// Gets or sets the bank name filter.
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Gets or sets the challan number filter.
    /// </summary>
    public string? ChallanNumber { get; set; }

    /// <summary>
    /// Gets or sets the TP number filter.
    /// </summary>
    public string? TPNumber { get; set; }

    /// <summary>
    /// Gets or sets the UTR number filter.
    /// </summary>
    public string? UTRNumber { get; set; }

    /// <summary>
    /// Gets or sets the PAN number filter.
    /// </summary>
    public string? PAN { get; set; }

    /// <summary>
    /// Gets or sets the payment status filter.
    /// </summary>
    public string? PaymentStatus { get; set; }

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
        VehicleId.HasValue ||
        MaterialId.HasValue ||
        !string.IsNullOrWhiteSpace(Driver) ||
        OwnerId.HasValue ||
        PaymentLocationId.HasValue ||
        !string.IsNullOrWhiteSpace(PaymentType) ||
        !string.IsNullOrWhiteSpace(Beneficiary) ||
        !string.IsNullOrWhiteSpace(BankName) ||
        !string.IsNullOrWhiteSpace(ChallanNumber) ||
        !string.IsNullOrWhiteSpace(TPNumber) ||
        !string.IsNullOrWhiteSpace(UTRNumber) ||
        !string.IsNullOrWhiteSpace(PAN) ||
        !string.IsNullOrWhiteSpace(PaymentStatus) ||
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
        VehicleId = null;
        MaterialId = null;
        Driver = null;
        OwnerId = null;
        PaymentLocationId = null;
        PaymentType = null;
        Beneficiary = null;
        BankName = null;
        ChallanNumber = null;
        TPNumber = null;
        UTRNumber = null;
        PAN = null;
        PaymentStatus = null;
        IsActive = null;
    }
}
