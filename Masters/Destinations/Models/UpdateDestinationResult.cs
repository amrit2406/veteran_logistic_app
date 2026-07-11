namespace veteran_logistic.Masters.Destinations.Models;

/// <summary>
/// Result model for destination update operations.
/// </summary>
public sealed class UpdateDestinationResult
{
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
    /// <returns>A successful result.</returns>
    public static UpdateDestinationResult Success()
    {
        return new UpdateDestinationResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateDestinationResult Failure(string errorMessage)
    {
        return new UpdateDestinationResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
