using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Contract for validating delete material requests.
/// </summary>
public interface IDeleteMaterialValidator
{
    /// <summary>
    /// Validates a delete material request.
    /// </summary>
    /// <param name="request">The delete material request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteMaterialRequest request);
}
