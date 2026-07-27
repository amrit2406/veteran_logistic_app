using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Validator interface for update unloading register requests.
/// </summary>
public interface IUpdateUnloadingRegisterValidator
{
    /// <summary>
    /// Validates an update unloading register request.
    /// </summary>
    /// <param name="request">The update unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateUnloadingRegisterRequest request);
}
