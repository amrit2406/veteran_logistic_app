namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Result of a payment register creation operation.
/// </summary>
public sealed class CreatePaymentRegisterResult
{
    /// <summary>
    /// Gets or sets the ID of the created payment register.
    /// </summary>
    public int PaymentRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the challan number of the created payment register.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="paymentRegisterId">The ID of the created payment register.</param>
    /// <param name="challanNumber">The challan number of the created payment register.</param>
    /// <returns>A successful result.</returns>
    public static CreatePaymentRegisterResult Success(int paymentRegisterId, string challanNumber)
    {
        return new CreatePaymentRegisterResult
        {
            PaymentRegisterId = paymentRegisterId,
            ChallanNumber = challanNumber,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreatePaymentRegisterResult Failure(string errorMessage)
    {
        return new CreatePaymentRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
