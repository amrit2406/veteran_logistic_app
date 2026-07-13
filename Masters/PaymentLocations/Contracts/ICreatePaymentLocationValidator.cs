using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Contract for validating create payment location requests.
/// </summary>
public interface ICreatePaymentLocationValidator
{
    /// <summary>
    /// Validates a create payment location request.
    /// </summary>
    /// <param name="request">The create payment location request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreatePaymentLocationRequest request);
}
