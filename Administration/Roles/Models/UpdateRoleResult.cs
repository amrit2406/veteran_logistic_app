namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Result model for role update operations.
/// </summary>
public sealed class UpdateRoleResult
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
    public static UpdateRoleResult Success()
    {
        return new UpdateRoleResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateRoleResult Failure(string errorMessage)
    {
        return new UpdateRoleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
