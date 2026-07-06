namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Result model for user update operations.
/// </summary>
public sealed class UpdateUserResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful.
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
    public static UpdateUserResult Success()
    {
        return new UpdateUserResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateUserResult Failure(string errorMessage)
    {
        return new UpdateUserResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
