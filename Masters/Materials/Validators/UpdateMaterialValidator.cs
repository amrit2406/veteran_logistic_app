using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Validators;

/// <summary>
/// Validator for material update requests.
/// </summary>
public sealed class UpdateMaterialValidator : IUpdateMaterialValidator
{
    private const int MaxMaterialNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateMaterialRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialRequest), "Update material request cannot be null."));
            return result;
        }

        if (request.MaterialId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialRequest.MaterialId), "Material ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.MaterialName))
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialRequest.MaterialName), "Material name is required."));
        }
        else if (request.MaterialName.Length > MaxMaterialNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateMaterialRequest.MaterialName), $"Material name must not exceed {MaxMaterialNameLength} characters."));
        }

        return result;
    }
}
