namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Request model for updating a party bill register.
/// </summary>
public sealed class UpdatePartyBillRegisterRequest
{
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

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
    /// Gets or sets the remarks.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who modified the party bill register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
