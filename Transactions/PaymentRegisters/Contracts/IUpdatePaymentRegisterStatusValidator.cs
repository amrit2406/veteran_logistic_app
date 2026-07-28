using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Validator interface for update payment register status requests.
/// </summary>
public interface IUpdatePaymentRegisterStatusValidator
{
    /// <summary>
    /// Validates an update payment register status request.
    /// </summary>
    /// <param name="request">The update payment register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the payment register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdatePaymentRegisterStatusRequest request, bool currentStatus);
}
