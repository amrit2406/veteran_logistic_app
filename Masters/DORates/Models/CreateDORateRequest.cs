namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// Request DTO for creating a new DO Rate.
/// </summary>
public sealed class CreateDORateRequest
{
    /// <summary>
    /// Gets or sets the consignor ID.
    /// </summary>
    public int ConsignorId { get; set; }

    /// <summary>
    /// Gets or sets the consignee ID.
    /// </summary>
    public int ConsigneeId { get; set; }

    /// <summary>
    /// Gets or sets the source location ID.
    /// </summary>
    public int SourceId { get; set; }

    /// <summary>
    /// Gets or sets the destination location ID.
    /// </summary>
    public int DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the effective date.
    /// </summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>
    /// Gets or sets the freight rate.
    /// </summary>
    public decimal FreightRate { get; set; }

    /// <summary>
    /// Gets or sets the union rate.
    /// </summary>
    public decimal UnionRate { get; set; }

    /// <summary>
    /// Gets or sets the vendor rate.
    /// </summary>
    public decimal VendorRate { get; set; }

    /// <summary>
    /// Gets or sets the DO number.
    /// </summary>
    public string DONumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the billing rate.
    /// </summary>
    public decimal BillingRate { get; set; }

    /// <summary>
    /// Gets or sets the allowed shortage.
    /// </summary>
    public decimal AllowedShortage { get; set; }

    /// <summary>
    /// Gets or sets the rate per kg.
    /// </summary>
    public decimal RatePerKg { get; set; }

    /// <summary>
    /// Gets or sets the vessel name.
    /// </summary>
    public string VesselName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the trader name.
    /// </summary>
    public string TraderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the narration.
    /// </summary>
    public string Narration { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the DO rate is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
