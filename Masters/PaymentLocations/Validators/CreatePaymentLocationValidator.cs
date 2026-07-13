using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Validators;

/// <summary>
/// Validates create payment location requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreatePaymentLocationValidator : ICreatePaymentLocationValidator
{
    private const int MaxPaymentLocationNameLength = 200;

    /// <summary>
    /// Validates a create payment location request.
    /// </summary>
    /// <param name="request">The create payment location request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreatePaymentLocationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreatePaymentLocationRequest), "Create payment location request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.PaymentLocationName))
        {
            result.AddError(new ValidationError(nameof(CreatePaymentLocationRequest.PaymentLocationName), "Payment location name is required."));
        }
        else if (request.PaymentLocationName.Length > MaxPaymentLocationNameLength)
        {
            result.AddError(new ValidationError(nameof(CreatePaymentLocationRequest.PaymentLocationName), $"Payment location name must not exceed {MaxPaymentLocationNameLength} characters."));
        }

        return result;
    }
}
