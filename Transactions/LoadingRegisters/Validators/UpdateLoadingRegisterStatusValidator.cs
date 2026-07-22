using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Validators;

/// <summary>
/// Validates update loading register status requests.
/// </summary>
public sealed class UpdateLoadingRegisterStatusValidator : IUpdateLoadingRegisterStatusValidator
{
    /// <summary>
    /// Validates an update loading register status request.
    /// </summary>
    /// <param name="request">The update loading register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the loading register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdateLoadingRegisterStatusRequest request, bool currentStatus)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterStatusRequest), "Update loading register status request cannot be null."));
            return result;
        }

        // Loading Register ID must be positive
        if (request.LoadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterStatusRequest.LoadingRegisterId), "Loading register ID must be positive."));
        }

        // Prevent redundant status changes
        if (request.IsActive == currentStatus)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterStatusRequest.IsActive), 
                request.IsActive ? "Loading register is already active." : "Loading register is already inactive."));
        }

        return result;
    }
}
