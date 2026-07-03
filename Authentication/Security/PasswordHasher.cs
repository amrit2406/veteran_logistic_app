using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Security;

/// <summary>
/// Implements password hashing and verification using PBKDF2.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8; // 128 bit salt
    private const int Iterations = 100000;
    private const int HashSize = 256 / 8; // 256 bit hash

    /// <summary>
    /// Hashes a password using PBKDF2 with HMAC-SHA256.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>A PasswordHashResult containing the hash and salt.</returns>
    public PasswordHashResult HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }

        // Generate a random salt
        var salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive the hash using PBKDF2
        var hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: HashSize);

        return new PasswordHashResult
        {
            Hash = Convert.ToBase64String(hash),
            Salt = Convert.ToBase64String(salt)
        };
    }

    /// <summary>
    /// Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="hash">The stored password hash.</param>
    /// <param name="salt">The stored salt used for hashing.</param>
    /// <returns>True if the password matches, otherwise false.</returns>
    public bool VerifyPassword(string password, string hash, string salt)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(salt))
        {
            return false;
        }

        try
        {
            var saltBytes = Convert.FromBase64String(salt);
            var storedHashBytes = Convert.FromBase64String(hash);

            // Derive the hash using the same parameters
            var computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            // Compare the hashes in constant time to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
        }
        catch
        {
            // If any conversion fails, return false
            return false;
        }
    }
}
