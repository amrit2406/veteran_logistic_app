using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Validator interface for update party bill register requests.
/// </summary>
public interface IUpdatePartyBillRegisterValidator
{
    /// <summary>
    /// Validates an update party bill register request.
    /// </summary>
    /// <param name="request">The update party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(Models.UpdatePartyBillRegisterRequest request);
}
