namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Result model for updating a party bill register's active status.
/// </summary>
public sealed class UpdatePartyBillRegisterStatusResult
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
