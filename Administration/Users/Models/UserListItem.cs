namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Represents a user item for display in the user listing grid.
/// </summary>
public sealed class UserListItem
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date the user was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }
}
