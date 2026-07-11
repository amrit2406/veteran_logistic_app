namespace veteran_logistic.Masters.Destinations.Models;

/// <summary>
/// Result model for destination creation operations.
/// </summary>
public sealed class CreateDestinationResult
{
    /// <summary>
    /// Gets or sets the ID of the created destination.
    /// </summary>
    public int DestinationId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="destinationId">The ID of the created destination.</param>
    /// <returns>A successful result.</returns>
    public static CreateDestinationResult Success(int destinationId)
    {
        return new CreateDestinationResult
        {
            DestinationId = destinationId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreateDestinationResult Failure(string errorMessage)
    {
        return new CreateDestinationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
