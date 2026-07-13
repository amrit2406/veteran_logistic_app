namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Result model for payment location status update operations.
/// </summary>
public sealed class UpdatePaymentLocationStatusResult
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
    public static UpdatePaymentLocationStatusResult Success()
    {
        return new UpdatePaymentLocationStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdatePaymentLocationStatusResult Failure(string errorMessage)
    {
        return new UpdatePaymentLocationStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
