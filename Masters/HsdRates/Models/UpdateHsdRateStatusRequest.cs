namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Request model for updating HSD rate active status.
/// </summary>
public sealed class UpdateHsdRateStatusRequest
{
    /// <summary>
    /// Gets or sets the HSD rate ID.
    /// </summary>
    public int HsdRateId { get; set; }

    /// <summary>
    /// Gets or sets whether the HSD rate is active.
    /// </summary>
    public bool IsActive { get; set; }
}
