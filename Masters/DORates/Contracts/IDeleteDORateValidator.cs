using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Validator interface for DO Rate deletion requests.
/// </summary>
public interface IDeleteDORateValidator
{
    /// <summary>
    /// Validates the deletion request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(DeleteDORateRequest request);
}
