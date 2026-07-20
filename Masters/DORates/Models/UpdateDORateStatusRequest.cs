namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// Request DTO for updating DO Rate active status.
/// </summary>
public sealed class UpdateDORateStatusRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the DO rate to update.
    /// </summary>
    public int DORateId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
