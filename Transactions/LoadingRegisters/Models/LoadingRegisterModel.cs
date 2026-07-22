namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Represents a loading register model for editing.
/// </summary>
public sealed class LoadingRegisterModel
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
    /// Gets or sets the consignor ID.
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignee ID.
    /// </summary>
    public int? ConsigneeId { get; set; }

    /// <summary>
    /// Gets or sets the source ID.
    /// </summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Gets or sets the destination ID.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int? VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the union/vendor ID.
    /// </summary>
    public int? UnionVendorId { get; set; }

    /// <summary>
    /// Gets or sets the driver commission.
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the gross weight.
    /// </summary>
    public decimal GrossWeight { get; set; }

    /// <summary>
    /// Gets or sets the tare weight.
    /// </summary>
    public decimal TareWeight { get; set; }

    /// <summary>
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the material ID.
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the rate.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets who loaded the vehicle.
    /// </summary>
    public string VehicleLoadedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fuel quantity.
    /// </summary>
    public decimal FuelQuantity { get; set; }

    /// <summary>
    /// Gets or sets the fuel amount.
    /// </summary>
    public decimal FuelAmount { get; set; }

    /// <summary>
    /// Gets or sets the fuel cash.
    /// </summary>
    public decimal FuelCash { get; set; }

    /// <summary>
    /// Gets or sets the fuel advance.
    /// </summary>
    public decimal FuelAdvance { get; set; }

    /// <summary>
    /// Gets or sets the shortage weight.
    /// </summary>
    public decimal ShortageWeight { get; set; }

    /// <summary>
    /// Gets or sets the cash advance.
    /// </summary>
    public decimal CashAdvance { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the other advance.
    /// </summary>
    public decimal OtherAdvance { get; set; }

    /// <summary>
    /// Gets or sets the other advance date.
    /// </summary>
    public DateTime? OtherAdvanceDate { get; set; }

    /// <summary>
    /// Gets or sets the third party.
    /// </summary>
    public string ThirdParty { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner ID.
    /// </summary>
    public int? OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the owner mobile.
    /// </summary>
    public string OwnerMobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner address.
    /// </summary>
    public string OwnerAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the driving licence number.
    /// </summary>
    public string DrivingLicenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the driver mobile.
    /// </summary>
    public string DriverMobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the loading register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
