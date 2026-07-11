namespace veteran_logistic.Masters.Destinations.Models;

/// <summary>
/// Request model for updating destination status.
/// </summary>
public sealed class UpdateDestinationStatusRequest
{
    /// <summary>
    /// Gets or sets the destination ID.
    /// </summary>
    public int DestinationId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
