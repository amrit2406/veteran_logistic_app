using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Validators;

/// <summary>
/// Validator for payment location status update requests.
/// </summary>
public sealed class UpdatePaymentLocationStatusValidator : IUpdatePaymentLocationStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdatePaymentLocationStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationStatusRequest), "Update payment location status request cannot be null."));
            return result;
        }

        if (request.PaymentLocationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationStatusRequest.PaymentLocationId), "Payment location ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Payment location is already active."
                : "Payment location is already inactive.";

            result.AddError(new ValidationError(nameof(UpdatePaymentLocationStatusRequest.IsActive), message));
        }

        return result;
    }
}
