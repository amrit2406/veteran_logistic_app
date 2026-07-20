namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// Result DTO for DO Rate creation operations.
/// </summary>
public sealed class CreateDORateResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the ID of the created DO rate.
    /// </summary>
    public int CreatedId { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
