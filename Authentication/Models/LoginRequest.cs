namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Represents a login request with username and password.
/// </summary>
public sealed class LoginRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
