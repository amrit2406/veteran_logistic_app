namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Represents a party bill register model for editing.
/// </summary>
public sealed class PartyBillRegisterModel
{
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number.
    /// </summary>
    public string BillNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Gets or sets the party/customer ID.
    /// </summary>
    public int PartyId { get; set; }

    /// <summary>
    /// Gets or sets the party/customer name.
    /// </summary>
    public string PartyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the third party name.
    /// </summary>
    public string ThirdPartyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string? PermitNumber { get; set; }

    /// <summary>
    /// Gets or sets the consignor ID filter.
    /// </summary>
    public int? ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignor name filter.
    /// </summary>
    public string? ConsignorName { get; set; }

    /// <summary>
    /// Gets or sets the destination ID filter.
    /// </summary>
    public int? DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the destination name filter.
    /// </summary>
    public string? DestinationName { get; set; }

    /// <summary>
    /// Gets or sets the from date filter.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Gets or sets the to date filter.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Gets or sets the total records.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the total material weight.
    /// </summary>
    public decimal TotalMaterialWeight { get; set; }

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the first charge head.
    /// </summary>
    public string? ChargeHead1 { get; set; }

    /// <summary>
    /// Gets or sets the first charge type.
    /// </summary>
    public string? ChargeType1 { get; set; }

    /// <summary>
    /// Gets or sets the first charge amount.
    /// </summary>
    public decimal ChargeAmount1 { get; set; }

    /// <summary>
    /// Gets or sets the second charge head.
    /// </summary>
    public string? ChargeHead2 { get; set; }

    /// <summary>
    /// Gets or sets the second charge type.
    /// </summary>
    public string? ChargeType2 { get; set; }

    /// <summary>
    /// Gets or sets the second charge amount.
    /// </summary>
    public decimal ChargeAmount2 { get; set; }

    /// <summary>
    /// Gets or sets the grand total.
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets the remarks.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the party bill register is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the party bill register details.
    /// </summary>
    public List<PartyBillRegisterDetailModel> PartyBillRegisterDetails { get; set; } = new();
}
