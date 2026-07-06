using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Validators;

/// <summary>
/// Validator for user update requests.
/// </summary>
public sealed class UpdateUserValidator : IUpdateUserValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateUserRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateUserRequest), "Update user request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            result.AddError(new ValidationError(nameof(UpdateUserRequest.DisplayName), "Display name is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Role))
        {
            result.AddError(new ValidationError(nameof(UpdateUserRequest.Role), "Role is required."));
        }

        return result;
    }
}
