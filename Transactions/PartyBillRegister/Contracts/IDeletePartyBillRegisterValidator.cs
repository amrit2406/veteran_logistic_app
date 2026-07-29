using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Validator interface for delete party bill register requests.
/// </summary>
public interface IDeletePartyBillRegisterValidator
{
    /// <summary>
    /// Validates a delete party bill register request.
    /// </summary>
    /// <param name="request">The delete party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(Models.DeletePartyBillRegisterRequest request);
}
