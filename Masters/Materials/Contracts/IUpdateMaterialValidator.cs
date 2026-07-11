using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Contract for validating update material requests.
/// </summary>
public interface IUpdateMaterialValidator
{
    /// <summary>
    /// Validates an update material request.
    /// </summary>
    /// <param name="request">The update material request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateMaterialRequest request);
}
