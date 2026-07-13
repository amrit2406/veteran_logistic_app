namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Request model for deleting a payment location.
/// </summary>
public sealed class DeletePaymentLocationRequest
{
    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int PaymentLocationId { get; set; }
}
