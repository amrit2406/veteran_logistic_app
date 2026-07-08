using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Contract for validating role status update requests.
/// </summary>
public interface IUpdateRoleStatusValidator
{
    /// <summary>
    /// Validates the role status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the role.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateRoleStatusRequest request, bool currentIsActive);
}
