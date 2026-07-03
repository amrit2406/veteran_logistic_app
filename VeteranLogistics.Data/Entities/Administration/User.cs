using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the salt used for password hashing.
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's role.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
