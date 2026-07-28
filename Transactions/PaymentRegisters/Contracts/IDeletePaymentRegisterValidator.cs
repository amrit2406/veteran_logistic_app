using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Validator interface for delete payment register requests.
/// </summary>
public interface IDeletePaymentRegisterValidator
{
    /// <summary>
    /// Validates a delete payment register request.
    /// </summary>
    /// <param name="request">The delete payment register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeletePaymentRegisterRequest request);
}
