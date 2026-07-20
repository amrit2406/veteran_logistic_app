using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Validator interface for source/destination status update requests.
/// </summary>
public interface IUpdateSourceDestinationStatusValidator
{
    /// <summary>
    /// Validates the status update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(UpdateSourceDestinationStatusRequest request);
}
