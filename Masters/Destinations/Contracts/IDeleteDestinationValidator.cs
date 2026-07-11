using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Contract for validating delete destination requests.
/// </summary>
public interface IDeleteDestinationValidator
{
    /// <summary>
    /// Validates a delete destination request.
    /// </summary>
    /// <param name="request">The delete destination request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteDestinationRequest request);
}
