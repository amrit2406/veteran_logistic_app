namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Request model for updating payment register status.
/// </summary>
public sealed class UpdatePaymentRegisterStatusRequest
{
    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int PaymentRegisterId { get; set; }

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; }
}
