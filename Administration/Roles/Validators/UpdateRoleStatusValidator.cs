using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Validators;

/// <summary>
/// Validator for role status update requests.
/// </summary>
public sealed class UpdateRoleStatusValidator : IUpdateRoleStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateRoleStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateRoleStatusRequest), "Update role status request cannot be null."));
            return result;
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Role is already active."
                : "Role is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateRoleStatusRequest.IsActive), message));
        }

        return result;
    }
}
