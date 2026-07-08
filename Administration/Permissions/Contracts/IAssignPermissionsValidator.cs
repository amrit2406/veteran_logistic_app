using veteran_logistic.Administration.Permissions.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Permissions.Contracts;

/// <summary>
/// Contract for validating assign permissions requests.
/// </summary>
public interface IAssignPermissionsValidator
{
    /// <summary>
    /// Validates an assign permissions request.
    /// </summary>
    /// <param name="request">The assign permissions request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(AssignPermissionsRequest request);
}
