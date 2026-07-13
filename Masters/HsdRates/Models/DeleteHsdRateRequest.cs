namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Request model for deleting an HSD rate (soft delete).
/// </summary>
public sealed class DeleteHsdRateRequest
{
    /// <summary>
    /// Gets or sets the HSD rate ID.
    /// </summary>
    public int HsdRateId { get; set; }
}
