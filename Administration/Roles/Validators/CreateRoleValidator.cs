using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Roles.Validators;

/// <summary>
/// Validates create role requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateRoleValidator : ICreateRoleValidator
{
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 500;

    /// <summary>
    /// Validates a create role request.
    /// </summary>
    /// <param name="request">The create role request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateRoleRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateRoleRequest), "Create role request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(CreateRoleRequest.Name), "Role name is required."));
        }
        else
        {
            var trimmedName = request.Name.Trim();
            if (trimmedName.Length == 0)
            {
                result.AddError(new ValidationError(nameof(CreateRoleRequest.Name), "Role name cannot be empty."));
            }
            else if (trimmedName.Length > MaxNameLength)
            {
                result.AddError(new ValidationError(nameof(CreateRoleRequest.Name), $"Role name must not exceed {MaxNameLength} characters."));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Description) && request.Description.Trim().Length > MaxDescriptionLength)
        {
            result.AddError(new ValidationError(nameof(CreateRoleRequest.Description), $"Description must not exceed {MaxDescriptionLength} characters."));
        }

        return result;
    }
}
