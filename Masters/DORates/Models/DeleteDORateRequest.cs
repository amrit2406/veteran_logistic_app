namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// Request DTO for deleting (soft deleting) a DO Rate.
/// </summary>
public sealed class DeleteDORateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the DO rate to delete.
    /// </summary>
    public int DORateId { get; set; }
}
