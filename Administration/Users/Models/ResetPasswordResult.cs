namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Result model for password reset operations.
/// </summary>
public sealed class ResetPasswordResult
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
    public static ResetPasswordResult Success()
    {
        return new ResetPasswordResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static ResetPasswordResult Failure(string errorMessage)
    {
        return new ResetPasswordResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
