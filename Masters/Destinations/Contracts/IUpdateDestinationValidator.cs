using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Contract for validating update destination requests.
/// </summary>
public interface IUpdateDestinationValidator
{
    /// <summary>
    /// Validates an update destination request.
    /// </summary>
    /// <param name="request">The update destination request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateDestinationRequest request);
}
