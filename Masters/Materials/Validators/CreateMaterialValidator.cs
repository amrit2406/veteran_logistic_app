using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Validators;

/// <summary>
/// Validates create material requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateMaterialValidator : ICreateMaterialValidator
{
    private const int MaxMaterialNameLength = 200;

    /// <summary>
    /// Validates a create material request.
    /// </summary>
    /// <param name="request">The create material request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateMaterialRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateMaterialRequest), "Create material request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.MaterialName))
        {
            result.AddError(new ValidationError(nameof(CreateMaterialRequest.MaterialName), "Material name is required."));
        }
        else if (request.MaterialName.Length > MaxMaterialNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateMaterialRequest.MaterialName), $"Material name must not exceed {MaxMaterialNameLength} characters."));
        }

        return result;
    }
}
