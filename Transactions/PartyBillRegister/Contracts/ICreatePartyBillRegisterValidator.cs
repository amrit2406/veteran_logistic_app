using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Validator interface for create party bill register requests.
/// </summary>
public interface ICreatePartyBillRegisterValidator
{
    /// <summary>
    /// Validates a create party bill register request.
    /// </summary>
    /// <param name="request">The create party bill register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(Models.CreatePartyBillRegisterRequest request);
}
