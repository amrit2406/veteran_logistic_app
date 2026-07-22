using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Validator interface for create loading register requests.
/// </summary>
public interface ICreateLoadingRegisterValidator
{
    /// <summary>
    /// Validates a create loading register request.
    /// </summary>
    /// <param name="request">The create loading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateLoadingRegisterRequest request);
}
