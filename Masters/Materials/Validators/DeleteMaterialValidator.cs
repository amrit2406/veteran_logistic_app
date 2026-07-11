using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Validators;

/// <summary>
/// Validates delete material requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteMaterialValidator : IDeleteMaterialValidator
{
    /// <summary>
    /// Validates a delete material request.
    /// </summary>
    /// <param name="request">The delete material request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteMaterialRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteMaterialRequest), "Delete material request cannot be null."));
            return result;
        }

        if (request.MaterialId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteMaterialRequest.MaterialId), "Material ID must be a positive value."));
        }

        return result;
    }
}
