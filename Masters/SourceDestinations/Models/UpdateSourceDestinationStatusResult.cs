namespace veteran_logistic.Masters.SourceDestinations.Models;

/// <summary>
/// Result DTO for source/destination status update operations.
/// </summary>
public sealed class UpdateSourceDestinationStatusResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
