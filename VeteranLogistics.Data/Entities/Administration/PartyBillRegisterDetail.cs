using VeteranLogistics.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a party bill register detail entry in the system.
/// </summary>
public class PartyBillRegisterDetail : BaseEntity
{
    /// <summary>
    /// Gets or sets the party bill register ID (foreign key to PartyBillRegister).
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the loading register ID (foreign key to LoadingRegister).
    /// </summary>
    public int LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the TP number from the loading register.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the challan number from the loading register.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number from the loading register.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the loading date from the loading register.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the material weight from the loading register.
    /// </summary>
    public decimal MaterialWeight { get; set; }

    /// <summary>
    /// Gets or sets the billing rate from the loading register.
    /// </summary>
    public decimal BillingRate { get; set; }

    /// <summary>
    /// Gets or sets the driver commission from the loading register.
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the amount from the loading register.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets whether the party bill register detail has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the party bill register detail was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    // Navigation properties
    /// <summary>
    /// Gets or sets the party bill register.
    /// </summary>
    [ForeignKey("PartyBillRegisterId")]
    public PartyBillRegister? PartyBillRegister { get; set; }

    /// <summary>
    /// Gets or sets the loading register.
    /// </summary>
    [ForeignKey("LoadingRegisterId")]
    public LoadingRegister? LoadingRegister { get; set; }
}
