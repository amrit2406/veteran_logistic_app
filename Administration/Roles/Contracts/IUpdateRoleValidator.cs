using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Contract for validating role update requests.
/// </summary>
public interface IUpdateRoleValidator
{
    /// <summary>
    /// Validates the role update request.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateRoleRequest request);
}
