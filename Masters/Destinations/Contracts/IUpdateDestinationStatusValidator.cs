using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Contract for validating destination status update requests.
/// </summary>
public interface IUpdateDestinationStatusValidator
{
    /// <summary>
    /// Validates the destination status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the destination.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateDestinationStatusRequest request, bool currentIsActive);
}
