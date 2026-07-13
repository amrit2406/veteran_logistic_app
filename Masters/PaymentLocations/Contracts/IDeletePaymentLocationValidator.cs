using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Contract for validating delete payment location requests.
/// </summary>
public interface IDeletePaymentLocationValidator
{
    /// <summary>
    /// Validates a delete payment location request.
    /// </summary>
    /// <param name="request">The delete payment location request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeletePaymentLocationRequest request);
}
