using VeteranLogistics.Data.Entities.Base;

using System.ComponentModel.DataAnnotations.Schema;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a payment register entry in the system.
/// </summary>
public class PaymentRegister : BaseEntity
{
    /// <summary>
    /// Gets or sets the challan number (from Loading/Unloading Register).
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading register ID (foreign key to LoadingRegister).
    /// </summary>
    public int? LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the unloading register ID (foreign key to UnloadingRegister).
    /// </summary>
    public int? UnloadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the TP number (from Loading/Unloading Register).
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number (from Loading/Unloading Register).
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the vehicle type (from Loading/Unloading Register).
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the material name (from Loading/Unloading Register).
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the driver commission (from Loading/Unloading Register).
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the loading date (from Loading Register).
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the unloading date (from Unloading Register).
    /// </summary>
    public DateTime? UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the loading weight (from Loading Register).
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight (from Unloading Register).
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID (foreign key to PaymentLocation).
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HSD party.
    /// </summary>
    public string? HSDParty { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTR number.
    /// </summary>
    public string UTRNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IFSC code.
    /// </summary>
    public string IFSCCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TDS percentage.
    /// </summary>
    public decimal TDSPercentage { get; set; }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the surcharge at 2%.
    /// </summary>
    public decimal Surcharge { get; set; }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal AdminCharge { get; set; }

    /// <summary>
    /// Gets or sets the gross amount (from Loading/Unloading Register).
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the payable amount (calculated).
    /// </summary>
    public decimal PayableAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public string PaymentStatus { get; set; } = "Pending";

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the payment register has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the payment register was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the payment register.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the payment register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    // Navigation properties
    /// <summary>
    /// Gets or sets the loading register.
    /// </summary>
    public LoadingRegister? LoadingRegister { get; set; }

    /// <summary>
    /// Gets or sets the unloading register.
    /// </summary>
    public UnloadingRegister? UnloadingRegister { get; set; }

    /// <summary>
    /// Gets or sets the payment location.
    /// </summary>
    public PaymentLocation? PaymentLocation { get; set; }
}
