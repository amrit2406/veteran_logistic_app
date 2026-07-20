namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// Request DTO for deleting (soft delete) a source/destination.
/// </summary>
public sealed class DeleteSourceDestinationRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the source/destination to delete.
    /// </summary>
    public int SourceDestinationId { get; set; }
}
