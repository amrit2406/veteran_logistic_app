using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Contract for validating HSD rate status update requests.
/// </summary>
public interface IUpdateHsdRateStatusValidator
{
    /// <summary>
    /// Validates the HSD rate status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the HSD rate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateHsdRateStatusRequest request, bool currentIsActive);
}
