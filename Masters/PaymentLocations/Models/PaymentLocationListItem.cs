namespace veteran_logistic.Masters.PaymentLocations.Models;

/// <summary>
/// Data transfer object for displaying payment location information in a list.
/// </summary>
public sealed class PaymentLocationListItem
{
    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string PaymentLocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the payment location is active.
    /// </summary>
    public bool IsActive { get; set; }
}
