using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for password policy validation.
/// </summary>
public interface IPasswordPolicy
{
    /// <summary>
    /// Validates a password against the policy requirements.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>A ValidationResult indicating whether the password meets policy requirements.</returns>
    ValidationResult ValidatePassword(string password);
}
