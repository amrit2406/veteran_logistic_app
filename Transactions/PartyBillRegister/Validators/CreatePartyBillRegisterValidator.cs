using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Validators;

/// <summary>
/// Validates create party bill register requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreatePartyBillRegisterValidator : ICreatePartyBillRegisterValidator
{
    /// <summary>
    /// Validates a create party bill register request.
    /// </summary>
    /// <param name="request">The create party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreatePartyBillRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreatePartyBillRegisterRequest), "Create party bill register request cannot be null."));
            return result;
        }

        // Party is required
        if (request.PartyId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreatePartyBillRegisterRequest.PartyId), "Party is required."));
        }

        // Bill Date is required
        if (request.BillDate == default)
        {
            result.AddError(new ValidationError(nameof(CreatePartyBillRegisterRequest.BillDate), "Bill date is required."));
        }

        // From Date cannot be greater than To Date
        if (request.FromDate.HasValue && request.ToDate.HasValue && request.FromDate.Value > request.ToDate.Value)
        {
            result.AddError(new ValidationError(nameof(CreatePartyBillRegisterRequest.FromDate), "From date cannot be greater than to date."));
        }

        // At least one loading register must be selected
        if (request.SelectedLoadingRegisterIds == null || request.SelectedLoadingRegisterIds.Count == 0)
        {
            result.AddError(new ValidationError(nameof(CreatePartyBillRegisterRequest.SelectedLoadingRegisterIds), "At least one loading register must be selected for the bill."));
        }

        return result;
    }
}
