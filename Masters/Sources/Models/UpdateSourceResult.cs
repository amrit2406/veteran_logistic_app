namespace veteran_logistic.Masters.Sources.Models;

/// <summary>
/// Result model for source update operations.
/// </summary>
public sealed class UpdateSourceResult
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
    public static UpdateSourceResult Success()
    {
        return new UpdateSourceResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateSourceResult Failure(string errorMessage)
    {
        return new UpdateSourceResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
