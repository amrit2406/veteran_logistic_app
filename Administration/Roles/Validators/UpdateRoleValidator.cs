using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Validators;

/// <summary>
/// Validator for role update requests.
/// </summary>
public sealed class UpdateRoleValidator : IUpdateRoleValidator
{
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 500;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateRoleRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateRoleRequest), "Update role request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(UpdateRoleRequest.Name), "Role name is required."));
        }
        else
        {
            var trimmedName = request.Name.Trim();
            if (trimmedName.Length == 0)
            {
                result.AddError(new ValidationError(nameof(UpdateRoleRequest.Name), "Role name cannot be empty."));
            }
            else if (trimmedName.Length > MaxNameLength)
            {
                result.AddError(new ValidationError(nameof(UpdateRoleRequest.Name), $"Role name must not exceed {MaxNameLength} characters."));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Trim().Length > MaxDescriptionLength)
        {
            result.AddError(new ValidationError(nameof(UpdateRoleRequest.Description), $"Description must not exceed {MaxDescriptionLength} characters."));
        }

        return result;
    }
}
