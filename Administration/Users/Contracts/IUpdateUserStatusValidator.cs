using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Contract for validating user status update requests.
/// </summary>
public interface IUpdateUserStatusValidator
{
    /// <summary>
    /// Validates the user status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the user.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateUserStatusRequest request, bool currentIsActive);
}
