using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Describes the authenticated user at runtime.
/// </summary>
public sealed class AuthenticatedUser
{
    /// <summary>
    /// Gets or sets the unique user identifier.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the username used by the current session.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the user-friendly display name.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user's current role name.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
