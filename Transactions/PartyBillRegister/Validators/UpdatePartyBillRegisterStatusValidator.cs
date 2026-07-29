using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Validators;

/// <summary>
/// Validates update party bill register status requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdatePartyBillRegisterStatusValidator : IUpdatePartyBillRegisterStatusValidator
{
    /// <summary>
    /// Validates an update party bill register status request.
    /// </summary>
    /// <param name="request">The update party bill register status request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdatePartyBillRegisterStatusRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterStatusRequest), "Update party bill register status request cannot be null."));
            return result;
        }

        // Party Bill Register ID is required
        if (request.PartyBillRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdatePartyBillRegisterStatusRequest.PartyBillRegisterId), "Party bill register ID is required."));
        }

        return result;
    }
}
