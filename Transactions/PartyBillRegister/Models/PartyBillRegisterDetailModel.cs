namespace veteran_logistic.Transactions.PartyBillRegister.Models;

/// <summary>
/// Represents a party bill register detail model.
/// </summary>
public sealed class PartyBillRegisterDetailModel
{
    /// <summary>
    /// Gets or sets the party bill register detail ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int LoadingRegisterId { get; set; }

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
}
