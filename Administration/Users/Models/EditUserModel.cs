namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Model representing user data for editing.
/// </summary>
public sealed class EditUserModel
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

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
    public bool IsActive { get; set; }
}
