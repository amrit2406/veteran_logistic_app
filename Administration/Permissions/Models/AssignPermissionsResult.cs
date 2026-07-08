namespace veteran_logistic.Administration.Permissions.Models;

/// <summary>
/// Result model for permission assignment operations.
/// </summary>
public sealed class AssignPermissionsResult
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
    public static AssignPermissionsResult Success()
    {
        return new AssignPermissionsResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static AssignPermissionsResult Failure(string errorMessage)
    {
        return new AssignPermissionsResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
