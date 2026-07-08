using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Contract for validating delete role requests.
/// </summary>
public interface IDeleteRoleValidator
{
    /// <summary>
    /// Validates a delete role request.
    /// </summary>
    /// <param name="request">The delete role request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteRoleRequest request);
}
