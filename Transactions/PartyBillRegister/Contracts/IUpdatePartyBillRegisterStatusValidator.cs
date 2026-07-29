using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Validator interface for update party bill register status requests.
/// </summary>
public interface IUpdatePartyBillRegisterStatusValidator
{
    /// <summary>
    /// Validates an update party bill register status request.
    /// </summary>
    /// <param name="request">The update party bill register status request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(Models.UpdatePartyBillRegisterStatusRequest request);
}
