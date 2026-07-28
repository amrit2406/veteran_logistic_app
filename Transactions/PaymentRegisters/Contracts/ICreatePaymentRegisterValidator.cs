using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Validator interface for create payment register requests.
/// </summary>
public interface ICreatePaymentRegisterValidator
{
    /// <summary>
    /// Validates a create payment register request.
    /// </summary>
    /// <param name="request">The create payment register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreatePaymentRegisterRequest request);
}
