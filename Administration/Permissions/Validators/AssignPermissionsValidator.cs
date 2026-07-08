using veteran_logistic.Administration.Permissions.Contracts;
using veteran_logistic.Administration.Permissions.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Permissions.Validators;

/// <summary>
/// Validates assign permissions requests to ensure required fields are present and valid.
/// </summary>
public sealed class AssignPermissionsValidator : IAssignPermissionsValidator
{
    /// <summary>
    /// Validates an assign permissions request.
    /// </summary>
    /// <param name="request">The assign permissions request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(AssignPermissionsRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(AssignPermissionsRequest), "Assign permissions request cannot be null."));
            return result;
        }

        if (request.RoleId <= 0)
        {
            result.AddError(new ValidationError(nameof(AssignPermissionsRequest.RoleId), "Role ID must be a positive value."));
        }

        if (request.PermissionAssignments is null)
        {
            result.AddError(new ValidationError(nameof(AssignPermissionsRequest.PermissionAssignments), "Permission assignments collection cannot be null."));
            return result;
        }

        // Check for duplicate permission IDs
        var duplicatePermissionIds = request.PermissionAssignments
            .GroupBy(pa => pa.PermissionId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicatePermissionIds.Any())
        {
            result.AddError(new ValidationError(nameof(AssignPermissionsRequest.PermissionAssignments), 
                $"Duplicate permission IDs found: {string.Join(", ", duplicatePermissionIds)}"));
        }

        // Validate each permission assignment item
        foreach (var assignment in request.PermissionAssignments)
        {
            if (assignment.PermissionId <= 0)
            {
                result.AddError(new ValidationError(nameof(PermissionAssignmentItem.PermissionId), 
                    $"Permission ID must be a positive value (found: {assignment.PermissionId})."));
            }
        }

        return result;
    }
}
