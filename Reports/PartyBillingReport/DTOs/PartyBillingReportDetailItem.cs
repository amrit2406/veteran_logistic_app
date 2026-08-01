namespace veteran_logistic.Reports.PartyBillingReport.DTOs;

/// <summary>
/// Represents a party billing report detail item for display in the detail grid.
/// </summary>
public sealed class PartyBillingReportDetailItem
{
    /// <summary>
    /// Gets or sets the party bill register detail ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string? Material { get; set; }

    /// <summary>
    /// Gets or sets the consignor name.
    /// </summary>
    public string? Consignor { get; set; }

    /// <summary>
    /// Gets or sets the consignee name.
    /// </summary>
    public string? Consignee { get; set; }

    /// <summary>
    /// Gets or sets the source name.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the destination name.
    /// </summary>
    public string? Destination { get; set; }

    /// <summary>
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the billing rate.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }
}
