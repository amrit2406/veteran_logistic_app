using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Validator interface for source/destination deletion requests.
/// </summary>
public interface IDeleteSourceDestinationValidator
{
    /// <summary>
    /// Validates the deletion request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(DeleteSourceDestinationRequest request);
}
