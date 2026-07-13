using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Validators;

/// <summary>
/// Validator for payment location update requests.
/// </summary>
public sealed class UpdatePaymentLocationValidator : IUpdatePaymentLocationValidator
{
    private const int MaxPaymentLocationNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdatePaymentLocationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationRequest), "Update payment location request cannot be null."));
            return result;
        }

        if (request.PaymentLocationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationRequest.PaymentLocationId), "Payment location ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.PaymentLocationName))
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationRequest.PaymentLocationName), "Payment location name is required."));
        }
        else if (request.PaymentLocationName.Length > MaxPaymentLocationNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentLocationRequest.PaymentLocationName), $"Payment location name must not exceed {MaxPaymentLocationNameLength} characters."));
        }

        return result;
    }
}
