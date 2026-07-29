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
    /// Gets or sets the remarks.
    /// </summary>
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who modified the party bill register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
