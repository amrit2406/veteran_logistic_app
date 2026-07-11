namespace veteran_logistic.Masters.Sources.Models;

/// <summary>
/// Result model for source creation operations.
/// </summary>
public sealed class CreateSourceResult
{
    /// <summary>
    /// Gets or sets the ID of the created source.
    /// </summary>
    public int SourceId { get; set; }

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
    /// <param name="sourceId">The ID of the created source.</param>
    /// <returns>A successful result.</returns>
    public static CreateSourceResult Success(int sourceId)
    {
        return new CreateSourceResult
        {
            SourceId = sourceId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreateSourceResult Failure(string errorMessage)
    {
        return new CreateSourceResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
