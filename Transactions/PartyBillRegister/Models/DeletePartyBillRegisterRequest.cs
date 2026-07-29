namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Request model for deleting a party bill register (soft delete).
/// </summary>
public sealed class DeletePartyBillRegisterRequest
{
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the user who deleted the party bill register.
    /// </summary>
    public string DeletedBy { get; set; } = string.Empty;
}
