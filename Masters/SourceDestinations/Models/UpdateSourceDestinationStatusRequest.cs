namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// Request DTO for updating source/destination active status.
/// </summary>
public sealed class UpdateSourceDestinationStatusRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the source/destination to update.
    /// </summary>
    public int SourceDestinationId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
