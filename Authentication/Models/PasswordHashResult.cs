namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Represents the result of a password hashing operation.
/// </summary>
public sealed class PasswordHashResult
{
    /// <summary>
    /// Gets or sets the hashed password.
    /// </summary>
    public string Hash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the salt used for hashing.
    /// </summary>
    public string Salt { get; set; } = string.Empty;
}
