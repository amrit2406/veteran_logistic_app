namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Result of a payment register delete operation.
/// </summary>
public sealed class DeletePaymentRegisterResult
{
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
    /// <returns>A successful result.</returns>
    public static DeletePaymentRegisterResult Success()
    {
        return new DeletePaymentRegisterResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeletePaymentRegisterResult Failure(string errorMessage)
    {
        return new DeletePaymentRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
