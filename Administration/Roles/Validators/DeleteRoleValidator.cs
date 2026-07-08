using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Validators;

/// <summary>
/// Validates delete role requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteRoleValidator : IDeleteRoleValidator
{
    /// <summary>
    /// Validates a delete role request.
    /// </summary>
    /// <param name="request">The delete role request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteRoleRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteRoleRequest), "Delete role request cannot be null."));
            return result;
        }

        if (request.RoleId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteRoleRequest.RoleId), "Role ID must be a positive value."));
        }

        return result;
    }
}
