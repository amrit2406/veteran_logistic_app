namespace veteran_logistic.Transactions.UnloadingRegisters.Models;

/// <summary>
/// Represents an unloading register item for display in the unloading register listing grid.
/// </summary>
public sealed class UnloadingRegisterListItem
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
    /// Gets or sets the loading register ID.
    /// </summary>
    public int? LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the loading register challan number.
    /// </summary>
    public string? LoadingRegisterChallanNumber { get; set; }

    /// <summary>
    /// Gets or sets the unloading date.
    /// </summary>
    public DateTime UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

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
    /// Gets or sets the material name.
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the gross weight at unloading.
    /// </summary>
    public decimal GrossWeightUL { get; set; }

    /// <summary>
    /// Gets or sets the tare weight at unloading.
    /// </summary>
    public decimal TareWeightUL { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight.
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the unloading register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
