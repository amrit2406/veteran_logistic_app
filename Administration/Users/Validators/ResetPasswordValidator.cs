using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Validators;

/// <summary>
/// Validates reset password requests to ensure required fields are present and valid.
/// </summary>
public sealed class ResetPasswordValidator : IResetPasswordValidator
{
    /// <summary>
    /// Validates a reset password request.
    /// </summary>
    /// <param name="request">The reset password request to validate.</param>
    /// <param name="userExists">Whether the user exists.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(ResetPasswordRequest request, bool userExists)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(ResetPasswordRequest), "Reset password request cannot be null."));
            return result;
        }

        if (!userExists)
        {
            result.AddError(new ValidationError(nameof(ResetPasswordRequest.UserId), "User not found."));
        }

        if (string.IsNullOrWhiteSpace(request.NewPassword))
        {
            result.AddError(new ValidationError(nameof(ResetPasswordRequest.NewPassword), "Password is required."));
        }

        if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
        {
            result.AddError(new ValidationError(nameof(ResetPasswordRequest.ConfirmPassword), "Confirm password is required."));
        }

        if (!string.IsNullOrWhiteSpace(request.NewPassword) && 
            !string.IsNullOrWhiteSpace(request.ConfirmPassword) && 
            request.NewPassword != request.ConfirmPassword)
        {
            result.AddError(new ValidationError(nameof(ResetPasswordRequest.ConfirmPassword), "Passwords do not match."));
        }

        return result;
    }
}
