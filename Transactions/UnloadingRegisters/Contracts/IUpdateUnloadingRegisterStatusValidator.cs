using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Validator interface for update unloading register status requests.
/// </summary>
public interface IUpdateUnloadingRegisterStatusValidator
{
    /// <summary>
    /// Validates an update unloading register status request.
    /// </summary>
    /// <param name="request">The update unloading register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the unloading register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateUnloadingRegisterStatusRequest request, bool currentStatus);
}
