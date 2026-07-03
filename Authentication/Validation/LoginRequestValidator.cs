using veteran_logistic.Authentication.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Authentication.Validation;

/// <summary>
/// Validates login requests to ensure required fields are present and valid.
/// </summary>
public static class LoginRequestValidator
{
    /// <summary>
    /// Validates a login request.
    /// </summary>
    /// <param name="request">The login request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public static ValidationResult Validate(LoginRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(LoginRequest), "Login request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            result.AddError(new ValidationError(nameof(LoginRequest.Username), "Username is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            result.AddError(new ValidationError(nameof(LoginRequest.Password), "Password is required."));
        }

        return result;
    }
}
