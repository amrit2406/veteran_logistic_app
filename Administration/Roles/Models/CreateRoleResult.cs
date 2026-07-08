namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Result model for role creation operations.
/// </summary>
public sealed class CreateRoleResult
{
    /// <summary>
    /// Gets or sets the ID of the created role.
    /// </summary>
    public int RoleId { get; set; }

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
    /// <param name="roleId">The ID of the created role.</param>
    /// <returns>A successful result.</returns>
    public static CreateRoleResult Success(int roleId)
    {
        return new CreateRoleResult
        {
            RoleId = roleId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateRoleResult Failure(string errorMessage)
    {
        return new CreateRoleResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
