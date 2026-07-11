using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Validators;

/// <summary>
/// Validator for material status update requests.
/// </summary>
public sealed class UpdateMaterialStatusValidator : IUpdateMaterialStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateMaterialStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialStatusRequest), "Update material status request cannot be null."));
            return result;
        }

        if (request.MaterialId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialStatusRequest.MaterialId), "Material ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Material is already active."
                : "Material is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateMaterialStatusRequest.IsActive), message));
        }

        return result;
    }
}
