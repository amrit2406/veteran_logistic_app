namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Result model for payment location creation operations.
/// </summary>
public sealed class CreatePaymentLocationResult
{
    /// <summary>
    /// Gets or sets the ID of the created payment location.
    /// </summary>
    public int PaymentLocationId { get; set; }

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
    /// <param name="paymentLocationId">The ID of the created payment location.</param>
    /// <returns>A successful result.</returns>
    public static CreatePaymentLocationResult Success(int paymentLocationId)
    {
        return new CreatePaymentLocationResult
        {
            PaymentLocationId = paymentLocationId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreatePaymentLocationResult Failure(string errorMessage)
    {
        return new CreatePaymentLocationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
