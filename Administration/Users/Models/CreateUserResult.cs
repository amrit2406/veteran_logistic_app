namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Result model for user creation operations.
/// </summary>
public sealed class CreateUserResult
{
    /// <summary>
    /// Gets or sets the ID of the created user.
    /// </summary>
    public int UserId { get; set; }

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
    /// <param name="userId">The ID of the created user.</param>
    /// <returns>A successful result.</returns>
    public static CreateUserResult Success(int userId)
    {
        return new CreateUserResult
        {
            UserId = userId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateUserResult Failure(string errorMessage)
    {
        return new CreateUserResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
