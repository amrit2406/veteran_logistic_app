namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Represents an eligible loading register for inclusion in a party bill.
/// </summary>
public sealed class EligibleLoadingRegisterModel
{
    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the material weight.
    /// </summary>
    public decimal MaterialWeight { get; set; }

    /// <summary>
    /// Gets or sets the billing rate.
    /// </summary>
    public decimal BillingRate { get; set; }

    /// <summary>
    /// Gets or sets the driver commission.
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets whether this loading register is selected for the bill.
    /// </summary>
    public bool IsSelected { get; set; }
}
