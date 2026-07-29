namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Result model for updating a party bill register.
/// </summary>
public sealed class UpdatePartyBillRegisterResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
