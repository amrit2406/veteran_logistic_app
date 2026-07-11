namespace veteran_logistic.Masters.Destinations.Models;

/// <summary>
/// Result model for destination status update operations.
/// </summary>
public sealed class UpdateDestinationStatusResult
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
    public static UpdateDestinationStatusResult Success()
    {
        return new UpdateDestinationStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateDestinationStatusResult Failure(string errorMessage)
    {
        return new UpdateDestinationStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
