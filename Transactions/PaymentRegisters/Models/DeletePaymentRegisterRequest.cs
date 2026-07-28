namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Request model for deleting a payment register.
/// </summary>
public sealed class DeletePaymentRegisterRequest
{
    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int PaymentRegisterId { get; set; }
}
