namespace veteran_logistic.Reports.UnloadingReport.DTOs;

/// <summary>
/// Represents an unloading report item for display in the unloading report grid.
/// </summary>
public sealed class UnloadingReportItem
{
    /// <summary>
    /// Gets or sets the unloading register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unloading date.
    /// </summary>
    public DateTime UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string? CustomerName { get; set; }

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
    /// Gets or sets the gross weight.
    /// </summary>
    public decimal GrossWeight { get; set; }

    /// <summary>
    /// Gets or sets the tare weight.
    /// </summary>
    public decimal TareWeight { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight.
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the shortage weight.
    /// </summary>
    public decimal ShortageWeight { get; set; }

    /// <summary>
    /// Gets or sets the rate.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the fuel amount.
    /// </summary>
    public decimal FuelAmount { get; set; }

    /// <summary>
    /// Gets or sets the cash advance.
    /// </summary>
    public decimal CashAdvance { get; set; }

    /// <summary>
    /// Gets or sets the other advance.
    /// </summary>
    public decimal OtherAdvance { get; set; }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string? PaymentLocationName { get; set; }

    /// <summary>
    /// Gets or sets the union/vendor name.
    /// </summary>
    public string? UnionVendorName { get; set; }

    /// <summary>
    /// Gets or sets the owner name.
    /// </summary>
    public string? OwnerName { get; set; }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status (active/inactive).
    /// </summary>
    public bool IsActive { get; set; }
}
