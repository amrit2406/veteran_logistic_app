namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Request model for updating an existing payment location.
/// </summary>
public sealed class UpdatePaymentLocationRequest
{
    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string PaymentLocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the payment location is active.
    /// </summary>
    public bool IsActive { get; set; }
}
