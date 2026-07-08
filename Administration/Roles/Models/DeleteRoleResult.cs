namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Result model for role deletion operations.
/// </summary>
public sealed class DeleteRoleResult
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
    public static DeleteRoleResult Success()
    {
        return new DeleteRoleResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteRoleResult Failure(string errorMessage)
    {
        return new DeleteRoleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
