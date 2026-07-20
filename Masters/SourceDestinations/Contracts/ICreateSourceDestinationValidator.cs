using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Validator interface for source/destination creation requests.
/// </summary>
public interface ICreateSourceDestinationValidator
{
    /// <summary>
    /// Validates the creation request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(CreateSourceDestinationRequest request);
}
