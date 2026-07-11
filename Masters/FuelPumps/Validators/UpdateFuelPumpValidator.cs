using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Validators;

/// <summary>
/// Validator for fuel pump update requests.
/// </summary>
public sealed class UpdateFuelPumpValidator : IUpdateFuelPumpValidator
{
    private const int MaxFuelPumpNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateFuelPumpRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpRequest), "Update fuel pump request cannot be null."));
            return result;
        }

        if (request.FuelPumpId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpRequest.FuelPumpId), "Fuel pump ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.FuelPumpName))
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpRequest.FuelPumpName), "Fuel pump name is required."));
        }
        else if (request.FuelPumpName.Length > MaxFuelPumpNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateFuelPumpRequest.FuelPumpName), $"Fuel pump name must not exceed {MaxFuelPumpNameLength} characters."));
        }

        return result;
    }
}
