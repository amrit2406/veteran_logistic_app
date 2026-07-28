using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Validators;

/// <summary>
/// Validates update payment register status requests to ensure the status change is valid.
/// </summary>
public sealed class UpdatePaymentRegisterStatusValidator : IUpdatePaymentRegisterStatusValidator
{
    /// <summary>
    /// Validates an update payment register status request.
    /// </summary>
    /// <param name="request">The update payment register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the payment register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdatePaymentRegisterStatusRequest request, bool currentStatus)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterStatusRequest), "Update payment register status request cannot be null."));
            return result;
        }

        // Payment Register ID is required
        if (request.PaymentRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterStatusRequest.PaymentRegisterId), "Payment register ID is required."));
        }

        // Prevent redundant status changes
        if (request.IsActive == currentStatus)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterStatusRequest.IsActive), $"Payment register is already {(currentStatus ? "active" : "inactive")}."));
        }

        return result;
    }
}
