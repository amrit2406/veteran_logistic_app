namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Represents a loading register item for display in the loading register listing grid.
/// </summary>
public sealed class LoadingRegisterListItem
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
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

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
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the loading register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
