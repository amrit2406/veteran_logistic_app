namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Represents a party bill register item for display in the party bill register listing grid.
/// </summary>
public sealed class PartyBillRegisterListItem
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
    /// Gets or sets the grand total.
    /// </summary>
    public decimal GrandTotal { get; set; }

    /// <summary>
    /// Gets or sets whether the party bill register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
