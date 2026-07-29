namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Result model for creating a party bill register.
/// </summary>
public sealed class CreatePartyBillRegisterResult
{
    /// <summary>
    /// Gets or sets the created party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the generated bill number.
    /// </summary>
    public string BillNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
