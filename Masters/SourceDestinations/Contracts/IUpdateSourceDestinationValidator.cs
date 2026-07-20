using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Validator interface for source/destination update requests.
/// </summary>
public interface IUpdateSourceDestinationValidator
{
    /// <summary>
    /// Validates the update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(UpdateSourceDestinationRequest request);
}
