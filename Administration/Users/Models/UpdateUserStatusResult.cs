namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Result model for user status update operations.
/// </summary>
public sealed class UpdateUserStatusResult
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
    public static UpdateUserStatusResult Success()
    {
        return new UpdateUserStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateUserStatusResult Failure(string errorMessage)
    {
        return new UpdateUserStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
