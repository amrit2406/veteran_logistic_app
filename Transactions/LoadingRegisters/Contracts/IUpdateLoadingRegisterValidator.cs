using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Validator interface for update loading register requests.
/// </summary>
public interface IUpdateLoadingRegisterValidator
{
    /// <summary>
    /// Validates an update loading register request.
    /// </summary>
    /// <param name="request">The update loading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateLoadingRegisterRequest request);
}
