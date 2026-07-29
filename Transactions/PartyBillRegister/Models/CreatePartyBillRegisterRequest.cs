namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Request model for creating a party bill register.
/// </summary>
public sealed class CreatePartyBillRegisterRequest
{
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
    /// Gets or sets the destination ID filter.
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
    /// Gets or sets the remarks.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the selected loading register IDs for the bill.
    /// </summary>
    public List<int> SelectedLoadingRegisterIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the user who created the party bill register.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}
