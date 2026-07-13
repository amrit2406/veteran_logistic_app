using veteran_logistic.Masters.PaymentLocations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Contract for validating update payment location requests.
/// </summary>
public interface IUpdatePaymentLocationValidator
{
    /// <summary>
    /// Validates an update payment location request.
    /// </summary>
    /// <param name="request">The update payment location request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdatePaymentLocationRequest request);
}
