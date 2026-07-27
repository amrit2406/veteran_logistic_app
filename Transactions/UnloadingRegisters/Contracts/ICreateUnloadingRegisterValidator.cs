using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Validator interface for create unloading register requests.
/// </summary>
public interface ICreateUnloadingRegisterValidator
{
    /// <summary>
    /// Validates a create unloading register request.
    /// </summary>
    /// <param name="request">The create unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateUnloadingRegisterRequest request);
}
