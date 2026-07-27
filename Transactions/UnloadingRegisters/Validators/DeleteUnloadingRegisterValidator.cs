using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Validators;

/// <summary>
/// Validates delete unloading register requests.
/// </summary>
public sealed class DeleteUnloadingRegisterValidator : IDeleteUnloadingRegisterValidator
{
    /// <summary>
    /// Validates a delete unloading register request.
    /// </summary>
    /// <param name="request">The delete unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteUnloadingRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteUnloadingRegisterRequest), "Delete unloading register request cannot be null."));
            return result;
        }

        // Unloading Register ID must be positive
        if (request.UnloadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteUnloadingRegisterRequest.UnloadingRegisterId), "Unloading register ID must be positive."));
        }

        return result;
    }
}
