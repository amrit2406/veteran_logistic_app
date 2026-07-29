using VeteranLogistics.Data.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a party bill register entry in the system.
/// </summary>
public class PartyBillRegister : BaseEntity
{
    /// <summary>
    /// Gets or sets the bill number (auto-generated).
    /// </summary>
    public string BillNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Gets or sets the party/customer ID (foreign key to Customer).
    /// </summary>
    public int PartyId { get; set; }

    /// <summary>
    /// Gets or sets the third party name (manual entry).
    /// </summary>
    public string ThirdPartyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the permit number (optional).
    /// </summary>
    public string? PermitNumber { get; set; }

    /// <summary>
    /// Gets or sets the consignor filter.
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the destination filter.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the from date filter.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Gets or sets the to date filter.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Gets or sets the total number of records in the bill.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the total material weight in the bill.
    /// </summary>
    public decimal TotalMaterialWeight { get; set; }

    /// <summary>
    /// Gets or sets the total amount in the bill.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the first additional charge head.
    /// </summary>
    public string? ChargeHead1 { get; set; }

    /// <summary>
    /// Gets or sets the first additional charge type.
    /// </summary>
    public string? ChargeType1 { get; set; }

    /// <summary>
    /// Gets or sets the first additional charge amount.
    /// </summary>
    public decimal ChargeAmount1 { get; set; }

    /// <summary>
    /// Gets or sets the second additional charge head.
    /// </summary>
    public string? ChargeHead2 { get; set; }

    /// <summary>
    /// Gets or sets the second additional charge type.
    /// </summary>
    public string? ChargeType2 { get; set; }

    /// <summary>
    /// Gets or sets the second additional charge amount.
    /// </summary>
    public decimal ChargeAmount2 { get; set; }

    /// <summary>
    /// Gets or sets the grand total (including additional charges).
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets the remarks for the bill.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the party bill register is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the party bill register has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the party bill register was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the party bill register.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the party bill register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;

    // Navigation properties
    /// <summary>
    /// Gets or sets the party/customer.
    /// </summary>
    [ForeignKey("PartyId")]
    public Customer? Party { get; set; }

    /// <summary>
    /// Gets or sets the consignor filter.
    /// </summary>
    public Customer? Consignor { get; set; }

    /// <summary>
    /// Gets or sets the destination filter.
    /// </summary>
    public SourceDestination? Destination { get; set; }

    /// <summary>
    /// Gets or sets the party bill register details.
    /// </summary>
    public ICollection<PartyBillRegisterDetail> PartyBillRegisterDetails { get; set; } = new List<PartyBillRegisterDetail>();
}
