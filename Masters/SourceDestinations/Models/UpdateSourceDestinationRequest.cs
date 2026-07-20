namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// Request DTO for updating an existing source/destination.
/// </summary>
public sealed class UpdateSourceDestinationRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the source/destination to update.
    /// </summary>
    public int SourceDestinationId { get; set; }

    /// <summary>
    /// Gets or sets the source/destination name.
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the source/destination is active.
    /// </summary>
    public bool IsActive { get; set; }
}
