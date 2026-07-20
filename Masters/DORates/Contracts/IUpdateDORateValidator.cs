using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Validator interface for DO Rate update requests.
/// </summary>
public interface IUpdateDORateValidator
{
    /// <summary>
    /// Validates the update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(UpdateDORateRequest request);
}
