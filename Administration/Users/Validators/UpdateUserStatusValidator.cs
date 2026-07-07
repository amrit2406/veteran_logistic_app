using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Validators;

/// <summary>
/// Validator for user status update requests.
/// </summary>
public sealed class UpdateUserStatusValidator : IUpdateUserStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateUserStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateUserStatusRequest), "Update user status request cannot be null."));
            return result;
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "User is already active."
                : "User is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateUserStatusRequest.IsActive), message));
        }

        return result;
    }
}
