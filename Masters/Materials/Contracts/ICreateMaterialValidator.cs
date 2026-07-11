using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Contract for validating create material requests.
/// </summary>
public interface ICreateMaterialValidator
{
    /// <summary>
    /// Validates a create material request.
    /// </summary>
    /// <param name="request">The create material request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateMaterialRequest request);
}
