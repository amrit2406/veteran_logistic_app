using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Validators;

/// <summary>
/// Validates delete payment register requests to ensure the deletion is valid.
/// </summary>
public sealed class DeletePaymentRegisterValidator : IDeletePaymentRegisterValidator
{
    /// <summary>
    /// Validates a delete payment register request.
    /// </summary>
    /// <param name="request">The delete payment register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeletePaymentRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeletePaymentRegisterRequest), "Delete payment register request cannot be null."));
            return result;
        }

        // Payment Register ID is required
        if (request.PaymentRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeletePaymentRegisterRequest.PaymentRegisterId), "Payment register ID is required."));
        }

        return result;
    }
}
