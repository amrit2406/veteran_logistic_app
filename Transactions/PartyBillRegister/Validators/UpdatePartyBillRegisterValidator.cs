using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Validators;

/// <summary>
/// Validates update party bill register requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdatePartyBillRegisterValidator : IUpdatePartyBillRegisterValidator
{
    /// <summary>
    /// Validates an update party bill register request.
    /// </summary>
    /// <param name="request">The update party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdatePartyBillRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterRequest), "Update party bill register request cannot be null."));
            return result;
        }

        // Party Bill Register ID is required
        if (request.PartyBillRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterRequest.PartyBillRegisterId), "Party bill register ID is required."));
        }

        // Party is required
        if (request.PartyId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterRequest.PartyId), "Party is required."));
        }

        // Bill Date is required
        if (request.BillDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterRequest.BillDate), "Bill date is required."));
        }

        return result;
    }
}
