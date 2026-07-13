namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Request model for updating payment location status.
/// </summary>
public sealed class UpdatePaymentLocationStatusRequest
{
    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
