namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Represents a payment register item for display in the payment register listing grid.
/// </summary>
public sealed class PaymentRegisterListItem
{
    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public string PaymentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gross amount.
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the payable amount.
    /// </summary>
    public decimal PayableAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
