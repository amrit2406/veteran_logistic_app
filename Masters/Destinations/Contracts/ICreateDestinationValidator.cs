using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Contract for validating create destination requests.
/// </summary>
public interface ICreateDestinationValidator
{
    /// <summary>
    /// Validates a create destination request.
    /// </summary>
    /// <param name="request">The create destination request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateDestinationRequest request);
}
