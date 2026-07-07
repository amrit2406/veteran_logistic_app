namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Result model for user deletion operations.
/// </summary>
public sealed class DeleteUserResult
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
    public static DeleteUserResult Success()
    {
        return new DeleteUserResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteUserResult Failure(string errorMessage)
    {
        return new DeleteUserResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
