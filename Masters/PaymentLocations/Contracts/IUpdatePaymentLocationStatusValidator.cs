using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Contract for validating payment location status update requests.
/// </summary>
public interface IUpdatePaymentLocationStatusValidator
{
    /// <summary>
    /// Validates the payment location status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the payment location.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdatePaymentLocationStatusRequest request, bool currentIsActive);
}
