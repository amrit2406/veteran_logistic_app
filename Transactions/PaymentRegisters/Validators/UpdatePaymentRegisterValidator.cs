using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PaymentRegisters.Validators;

/// <summary>
/// Validates update payment register requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdatePaymentRegisterValidator : IUpdatePaymentRegisterValidator
{
    /// <summary>
    /// Validates an update payment register request.
    /// </summary>
    /// <param name="request">The update payment register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdatePaymentRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest), "Update payment register request cannot be null."));
            return result;
        }

        // Payment Register ID is required
        if (request.PaymentRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.PaymentRegisterId), "Payment register ID is required."));
        }

        // Challan Number is required
        if (string.IsNullOrWhiteSpace(request.ChallanNumber))
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.ChallanNumber), "Challan number is required."));
        }

        // Payment Date is required
        if (request.PaymentDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.PaymentDate), "Payment date is required."));
        }

        // Payment Location is required
        if (request.PaymentLocationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.PaymentLocationId), "Payment location is required."));
        }

        // Payment Type is required
        if (string.IsNullOrWhiteSpace(request.PaymentType))
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.PaymentType), "Payment type is required."));
        }

        // Beneficiary is required
        if (string.IsNullOrWhiteSpace(request.Beneficiary))
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.Beneficiary), "Beneficiary is required."));
        }

        // TDS Percentage cannot be negative
        if (request.TDSPercentage < 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.TDSPercentage), "TDS percentage cannot be negative."));
        }

        // Challan Money cannot be negative
        if (request.ChallanMoney < 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.ChallanMoney), "Challan money cannot be negative."));
        }

        // Surcharge cannot be negative
        if (request.Surcharge < 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.Surcharge), "Surcharge cannot be negative."));
        }

        // Admin Charge cannot be negative
        if (request.AdminCharge < 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePaymentRegisterRequest.AdminCharge), "Admin charge cannot be negative."));
        }

        return result;
    }
}
