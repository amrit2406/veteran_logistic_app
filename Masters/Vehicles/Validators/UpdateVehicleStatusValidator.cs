using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Validators;

/// <summary>
/// Validator for vehicle status update requests.
/// </summary>
public sealed class UpdateVehicleStatusValidator : IUpdateVehicleStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateVehicleStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        // Validate Vehicle ID
        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleId), "Vehicle ID must be positive."));
        }

        // Validate meaningful status change
        if (request.IsActive == currentIsActive)
        {
            result.AddError(new ValidationError(nameof(request.IsActive), "Status is already set to the requested value."));
        }

        return result;
    }
}
