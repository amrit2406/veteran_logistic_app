using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for password hashing and verification operations.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a password using PBKDF2.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>A PasswordHashResult containing the hash and salt.</returns>
    PasswordHashResult HashPassword(string password);

    /// <summary>
    /// Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="hash">The stored password hash.</param>
    /// <param name="salt">The stored salt used for hashing.</param>
    /// <returns>True if the password matches, otherwise false.</returns>
    bool VerifyPassword(string password, string hash, string salt);
}
