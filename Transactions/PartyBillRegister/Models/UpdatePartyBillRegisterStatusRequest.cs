namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Request model for updating a party bill register's active status.
/// </summary>
public sealed class UpdatePartyBillRegisterStatusRequest
{
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets whether the party bill register should be active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the user who modified the party bill register.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
