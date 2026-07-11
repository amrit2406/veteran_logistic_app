namespace veteran_logistic.Masters.Sources.Models;

/// <summary>
/// Request model for updating source status.
/// </summary>
public sealed class UpdateSourceStatusRequest
{
    /// <summary>
    /// Gets or sets the source ID.
    /// </summary>
    public int SourceId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
