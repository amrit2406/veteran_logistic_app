namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// Request DTO for creating a new source/destination.
/// </summary>
public sealed class CreateSourceDestinationRequest
{
    /// <summary>
    /// Gets or sets the source/destination name.
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the source/destination is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
