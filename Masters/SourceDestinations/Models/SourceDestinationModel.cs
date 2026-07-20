namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// DTO for detailed source/destination information, used for editing.
/// </summary>
public sealed class SourceDestinationModel
{
    /// <summary>
    /// Gets or sets the unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the source/destination name.
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the source/destination is active.
    /// </summary>
    public bool IsActive { get; set; }
}
