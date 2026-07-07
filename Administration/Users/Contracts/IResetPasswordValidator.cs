using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Contract for validating reset password requests.
/// </summary>
public interface IResetPasswordValidator
{
    /// <summary>
    /// Validates a reset password request.
    /// </summary>
    /// <param name="request">The reset password request to validate.</param>
    /// <param name="userExists">Whether the user exists.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(ResetPasswordRequest request, bool userExists);
}
