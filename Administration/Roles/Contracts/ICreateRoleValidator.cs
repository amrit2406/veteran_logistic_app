using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Contract for validating create role requests.
/// </summary>
public interface ICreateRoleValidator
{
    /// <summary>
    /// Validates a create role request.
    /// </summary>
    /// <param name="request">The create role request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateRoleRequest request);
}
