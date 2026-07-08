namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Result model for role status update operations.
/// </summary>
public sealed class UpdateRoleStatusResult
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
    public static UpdateRoleStatusResult Success()
    {
        return new UpdateRoleStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateRoleStatusResult Failure(string errorMessage)
    {
        return new UpdateRoleStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
