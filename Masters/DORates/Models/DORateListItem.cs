namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// DTO for displaying DO Rate information in lists.
/// </summary>
public sealed class DORateListItem
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the consignor name (resolved from ID).
    /// </summary>
    public string Consignor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the consignee name (resolved from ID).
    /// </summary>
    public string Consignee { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source location name.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination location name.
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the effective date.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Gets or sets the freight rate.
    /// </summary>
    public decimal FreightRate { get; set; }

    /// <summary>
    /// Gets or sets the vendor rate.
    /// </summary>
    public decimal VendorRate { get; set; }

    /// <summary>
    /// Gets or sets the billing rate.
    /// </summary>
    public decimal BillingRate { get; set; }

    /// <summary>
    /// Gets or sets the DO number.
    /// </summary>
    public string DONumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the DO rate is active.
    /// </summary>
    public bool IsActive { get; set; }
}
