namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Request model for updating an existing user.
/// </summary>
public sealed class UpdateUserRequest
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
