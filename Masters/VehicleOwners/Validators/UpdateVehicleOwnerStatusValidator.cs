using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Validators;

/// <summary>
/// Validator for vehicle owner status update requests.
/// </summary>
public sealed class UpdateVehicleOwnerStatusValidator : IUpdateVehicleOwnerStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateVehicleOwnerStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        // Validate Vehicle Owner ID
        if (request.VehicleOwnerId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleOwnerId), "Vehicle Owner ID must be positive."));
        }

        // Validate meaningful status change
        if (request.IsActive == currentIsActive)
        {
            result.AddError(new ValidationError(nameof(request.IsActive), "Status is already set to the requested value."));
        }

        return result;
    }
}
