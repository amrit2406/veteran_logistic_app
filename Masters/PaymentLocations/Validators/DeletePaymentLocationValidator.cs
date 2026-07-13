using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Validators;

/// <summary>
/// Validates delete payment location requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeletePaymentLocationValidator : IDeletePaymentLocationValidator
{
    /// <summary>
    /// Validates a delete payment location request.
    /// </summary>
    /// <param name="request">The delete payment location request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeletePaymentLocationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeletePaymentLocationRequest), "Delete payment location request cannot be null."));
            return result;
        }

        if (request.PaymentLocationId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeletePaymentLocationRequest.PaymentLocationId), "Payment location ID must be a positive value."));
        }

        return result;
    }
}
