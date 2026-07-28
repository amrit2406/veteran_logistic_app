namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Result of a payment register update operation.
/// </summary>
public sealed class UpdatePaymentRegisterResult
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
    public static UpdatePaymentRegisterResult Success()
    {
        return new UpdatePaymentRegisterResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdatePaymentRegisterResult Failure(string errorMessage)
    {
        return new UpdatePaymentRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
