using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Validator interface for delete unloading register requests.
/// </summary>
public interface IDeleteUnloadingRegisterValidator
{
    /// <summary>
    /// Validates a delete unloading register request.
    /// </summary>
    /// <param name="request">The delete unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteUnloadingRegisterRequest request);
}
