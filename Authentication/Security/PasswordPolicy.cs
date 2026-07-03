using System.Text.RegularExpressions;
using veteran_logistic.Authentication.Contracts;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Authentication.Security;

/// <summary>
/// Implements password policy validation rules.
/// </summary>
public sealed class PasswordPolicy : IPasswordPolicy
{
    private const int MinimumLength = 8;
    private const int MaximumLength = 128;

    /// <summary>
    /// Validates a password against the policy requirements.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>A ValidationResult indicating whether the password meets policy requirements.</returns>
    public ValidationResult ValidatePassword(string password)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(password))
        {
            result.AddError(new ValidationError(nameof(password), "Password is required."));
            return result;
        }

        if (password.Length < MinimumLength)
        {
            result.AddError(new ValidationError(nameof(password), $"Password must be at least {MinimumLength} characters long."));
        }

        if (password.Length > MaximumLength)
        {
            result.AddError(new ValidationError(nameof(password), $"Password must not exceed {MaximumLength} characters."));
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            result.AddError(new ValidationError(nameof(password), "Password must contain at least one uppercase letter."));
        }

        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            result.AddError(new ValidationError(nameof(password), "Password must contain at least one lowercase letter."));
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            result.AddError(new ValidationError(nameof(password), "Password must contain at least one digit."));
        }

        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
        {
            result.AddError(new ValidationError(nameof(password), "Password must contain at least one special character."));
        }

        return result;
    }
}
