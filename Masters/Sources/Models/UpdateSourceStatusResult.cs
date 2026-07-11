namespace veteran_logistic.Masters.Sources.Models;

/// <summary>
/// Result model for source status update operations.
/// </summary>
public sealed class UpdateSourceStatusResult
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
    public static UpdateSourceStatusResult Success()
    {
        return new UpdateSourceStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateSourceStatusResult Failure(string errorMessage)
    {
        return new UpdateSourceStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
