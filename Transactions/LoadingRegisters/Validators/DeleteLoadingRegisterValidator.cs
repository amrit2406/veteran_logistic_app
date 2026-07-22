using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Validators;

/// <summary>
/// Validates delete loading register requests.
/// </summary>
public sealed class DeleteLoadingRegisterValidator : IDeleteLoadingRegisterValidator
{
    /// <summary>
    /// Validates a delete loading register request.
    /// </summary>
    /// <param name="request">The delete loading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteLoadingRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteLoadingRegisterRequest), "Delete loading register request cannot be null."));
            return result;
        }

        // Loading Register ID must be positive
        if (request.LoadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteLoadingRegisterRequest.LoadingRegisterId), "Loading register ID must be positive."));
        }

        return result;
    }
}
