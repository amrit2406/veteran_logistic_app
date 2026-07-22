using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Validator interface for delete loading register requests.
/// </summary>
public interface IDeleteLoadingRegisterValidator
{
    /// <summary>
    /// Validates a delete loading register request.
    /// </summary>
    /// <param name="request">The delete loading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteLoadingRegisterRequest request);
}
