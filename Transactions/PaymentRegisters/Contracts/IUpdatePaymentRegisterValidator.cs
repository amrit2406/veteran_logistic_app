using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Validator interface for update payment register requests.
/// </summary>
public interface IUpdatePaymentRegisterValidator
{
    /// <summary>
    /// Validates an update payment register request.
    /// </summary>
    /// <param name="request">The update payment register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdatePaymentRegisterRequest request);
}
