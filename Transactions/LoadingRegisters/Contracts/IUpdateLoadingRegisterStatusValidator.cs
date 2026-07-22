using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Validator interface for update loading register status requests.
/// </summary>
public interface IUpdateLoadingRegisterStatusValidator
{
    /// <summary>
    /// Validates an update loading register status request.
    /// </summary>
    /// <param name="request">The update loading register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the loading register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateLoadingRegisterStatusRequest request, bool currentStatus);
}
