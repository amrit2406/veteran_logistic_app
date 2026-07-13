namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Request model for creating a new payment location.
/// </summary>
public sealed class CreatePaymentLocationRequest
{
    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string PaymentLocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the payment location is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
