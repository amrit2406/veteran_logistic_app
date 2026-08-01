namespace veteran_logistic.Reports.TdsReport.DTOs;

/// <summary>
/// Represents filter criteria for the TDS report.
/// </summary>
public sealed class TdsReportFilter
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
    /// Gets or sets the driver name filter.
    /// </summary>
    public string? Driver { get; set; }

    /// <summary>
    /// Gets or sets the beneficiary filter.
    /// </summary>
    public string? Beneficiary { get; set; }

    /// <summary>
    /// Gets or sets the PAN number filter.
    /// </summary>
    public string? PAN { get; set; }

    /// <summary>
    /// Gets or sets the bank name filter.
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Gets or sets the payment type filter.
    /// </summary>
    public string? PaymentType { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID filter.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the TDS percentage filter.
    /// </summary>
    public decimal? TDSPercentage { get; set; }

    /// <summary>
    /// Gets or sets the payment status filter.
    /// </summary>
    public string? PaymentStatus { get; set; }

    /// <summary>
    /// Determines whether the filter has any active criteria.
    /// </summary>
    public bool HasFilter =>
        DateFrom.HasValue ||
        DateTo.HasValue ||
        CompanyId.HasValue ||
        CustomerId.HasValue ||
        VehicleId.HasValue ||
        !string.IsNullOrWhiteSpace(Driver) ||
        !string.IsNullOrWhiteSpace(Beneficiary) ||
        !string.IsNullOrWhiteSpace(PAN) ||
        !string.IsNullOrWhiteSpace(BankName) ||
        !string.IsNullOrWhiteSpace(PaymentType) ||
        PaymentLocationId.HasValue ||
        TDSPercentage.HasValue ||
        !string.IsNullOrWhiteSpace(PaymentStatus);

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
        Driver = null;
        Beneficiary = null;
        PAN = null;
        BankName = null;
        PaymentType = null;
        PaymentLocationId = null;
        TDSPercentage = null;
        PaymentStatus = null;
    }
}
