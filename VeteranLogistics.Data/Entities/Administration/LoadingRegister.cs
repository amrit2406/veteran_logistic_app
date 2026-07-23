using VeteranLogistics.Data.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a loading register entry in the system.
/// </summary>
public class LoadingRegister : BaseEntity
{
    /// <summary>
    /// Gets or sets the challan number (auto-generated).
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the consignor ID (foreign key to Customer).
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignee ID (foreign key to Customer).
    /// </summary>
    public int? ConsigneeId { get; set; }

    /// <summary>
    /// Gets or sets the source ID (foreign key to SourceDestination).
    /// </summary>
    public int? SourceId { get; set; }

    /// <summary>
    /// Gets or sets the destination ID (foreign key to SourceDestination).
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
    /// Gets or sets the vehicle ID (foreign key to Vehicle).
    /// </summary>
    public int? VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle type (Union or Vendor).
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the union/vendor ID (foreign key to Vendor).
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
    /// Gets or sets the loading weight (calculated: Gross - Tare).
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the material ID (foreign key to Material).
    /// </summary>
    public int? MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the rate.
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Gets or sets the gross amount (calculated: LoadingWeight × Rate).
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
    /// Gets or sets the payment location ID (foreign key to PaymentLocation).
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
    /// Gets or sets the owner ID (foreign key to VehicleOwner).
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
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the loading register has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the loading register was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the loading register.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the loading register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    // Navigation properties
    /// <summary>
    /// Gets or sets the consignor (Customer).
    /// </summary>
    [ForeignKey("ConsignorId")]
    public Customer? Consignor { get; set; }

    /// <summary>
    /// Gets or sets the consignee (Customer).
    /// </summary>
    [ForeignKey("ConsigneeId")]
    public Customer? Consignee { get; set; }

    /// <summary>
    /// Gets or sets the source (SourceDestination).
    /// </summary>
    public SourceDestination? Source { get; set; }

    /// <summary>
    /// Gets or sets the destination (SourceDestination).
    /// </summary>
    public SourceDestination? Destination { get; set; }

    /// <summary>
    /// Gets or sets the vehicle.
    /// </summary>
    public Vehicle? Vehicle { get; set; }

    /// <summary>
    /// Gets or sets the union/vendor.
    /// </summary>
    public Vendor? UnionVendor { get; set; }

    /// <summary>
    /// Gets or sets the material.
    /// </summary>
    public Material? Material { get; set; }

    /// <summary>
    /// Gets or sets the payment location.
    /// </summary>
    public PaymentLocation? PaymentLocation { get; set; }

    /// <summary>
    /// Gets or sets the owner.
    /// </summary>
    public VehicleOwner? Owner { get; set; }
}
