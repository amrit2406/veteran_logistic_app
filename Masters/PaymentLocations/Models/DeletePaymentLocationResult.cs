namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Result model for payment location delete operations.
/// </summary>
public sealed class DeletePaymentLocationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
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
    public static DeletePaymentLocationResult Success()
    {
        return new DeletePaymentLocationResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static DeletePaymentLocationResult Failure(string errorMessage)
    {
        return new DeletePaymentLocationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
