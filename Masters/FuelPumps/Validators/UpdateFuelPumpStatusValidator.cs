using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Validators;

/// <summary>
/// Validator for fuel pump status update requests.
/// </summary>
public sealed class UpdateFuelPumpStatusValidator : IUpdateFuelPumpStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateFuelPumpStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpStatusRequest), "Update fuel pump status request cannot be null."));
            return result;
        }

        if (request.FuelPumpId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpStatusRequest.FuelPumpId), "Fuel pump ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Fuel pump is already active."
                : "Fuel pump is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateFuelPumpStatusRequest.IsActive), message));
        }

        return result;
    }
}
