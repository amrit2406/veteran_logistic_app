using veteran_logistic.Masters.Materials.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Contract for validating material status update requests.
/// </summary>
public interface IUpdateMaterialStatusValidator
{
    /// <summary>
    /// Validates the material status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the material.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateMaterialStatusRequest request, bool currentIsActive);
}
