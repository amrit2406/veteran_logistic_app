using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Validator interface for DO Rate creation requests.
/// </summary>
public interface ICreateDORateValidator
{
    /// <summary>
    /// Validates the creation request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(CreateDORateRequest request);
}
