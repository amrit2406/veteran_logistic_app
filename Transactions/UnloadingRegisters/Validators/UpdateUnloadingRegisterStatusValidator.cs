using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Validators;

/// <summary>
/// Validates update unloading register status requests.
/// </summary>
public sealed class UpdateUnloadingRegisterStatusValidator : IUpdateUnloadingRegisterStatusValidator
{
    /// <summary>
    /// Validates an update unloading register status request.
    /// </summary>
    /// <param name="request">The update unloading register status request to validate.</param>
    /// <param name="currentStatus">The current active status of the unloading register.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdateUnloadingRegisterStatusRequest request, bool currentStatus)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterStatusRequest), "Update unloading register status request cannot be null."));
            return result;
        }

        // Unloading Register ID must be positive
        if (request.UnloadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterStatusRequest.UnloadingRegisterId), "Unloading register ID must be positive."));
        }

        // Prevent redundant status changes
        if (request.IsActive == currentStatus)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterStatusRequest.IsActive), 
                request.IsActive ? "Unloading register is already active." : "Unloading register is already inactive."));
        }

        return result;
    }
}
