using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Validators;

/// <summary>
/// Validates delete party bill register requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeletePartyBillRegisterValidator : IDeletePartyBillRegisterValidator
{
    /// <summary>
    /// Validates a delete party bill register request.
    /// </summary>
    /// <param name="request">The delete party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeletePartyBillRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeletePartyBillRegisterRequest), "Delete party bill register request cannot be null."));
            return result;
        }

        // Party Bill Register ID is required
        if (request.PartyBillRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeletePartyBillRegisterRequest.PartyBillRegisterId), "Party bill register ID is required."));
        }

        return result;
    }
}
