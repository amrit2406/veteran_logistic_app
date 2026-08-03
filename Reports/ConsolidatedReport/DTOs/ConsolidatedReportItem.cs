namespace veteran_logistic.Reports.ConsolidatedReport.DTOs;

/// <summary>
/// Represents a consolidated report item that combines data from Loading, Unloading, Payment, and Billing stages.
/// </summary>
public sealed class ConsolidatedReportItem
{
    // Loading Information
    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string? MaterialName { get; set; }

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
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the rate.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the loading amount (gross amount).
    /// </summary>
    public decimal LoadingAmount { get; set; }

    // Unloading Information
    /// <summary>
    /// Gets or sets the unloading register ID.
    /// </summary>
    public int? UnloadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the unloading date.
    /// </summary>
    public DateTime? UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight.
    /// </summary>
    public decimal? UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the shortage weight.
    /// </summary>
    public decimal? ShortageWeight { get; set; }

    // Payment Information
    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int? PaymentRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string? Beneficiary { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string? PaymentType { get; set; }

    /// <summary>
    /// Gets or sets the driver commission.
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the challan amount.
    /// </summary>
    public decimal? ChallanAmount { get; set; }

    /// <summary>
    /// Gets or sets the TDS amount.
    /// </summary>
    public decimal? TDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the surcharge amount.
    /// </summary>
    public decimal? Surcharge { get; set; }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal? AdminCharge { get; set; }

    /// <summary>
    /// Gets or sets the net payment amount.
    /// </summary>
    public decimal? NetPayment { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public string? PaymentStatus { get; set; }

    // Billing Information
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int? PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the party bill register detail ID.
    /// </summary>
    public int? PartyBillRegisterDetailId { get; set; }

    /// <summary>
    /// Gets or sets the bill number.
    /// </summary>
    public string? BillNumber { get; set; }

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime? BillDate { get; set; }

    /// <summary>
    /// Gets or sets the customer/party name.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the third party name.
    /// </summary>
    public string? ThirdParty { get; set; }

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string? PermitNumber { get; set; }

    /// <summary>
    /// Gets or sets the billing status.
    /// </summary>
    public string? BillingStatus { get; set; }

    // Common Information
    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner name.
    /// </summary>
    public string? OwnerName { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string? PaymentLocationName { get; set; }

    /// <summary>
    /// Gets or sets the lifecycle status based on actual data.
    /// </summary>
    public string LifecycleStatus { get; set; } = string.Empty;
}
