using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Validators;

/// <summary>
/// Validates create fuel pump requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateFuelPumpValidator : ICreateFuelPumpValidator
{
    private const int MaxFuelPumpNameLength = 200;

    /// <summary>
    /// Validates a create fuel pump request.
    /// </summary>
    /// <param name="request">The create fuel pump request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateFuelPumpRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateFuelPumpRequest), "Create fuel pump request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.FuelPumpName))
        {
            result.AddError(new ValidationError(nameof(CreateFuelPumpRequest.FuelPumpName), "Fuel pump name is required."));
        }
        else if (request.FuelPumpName.Length > MaxFuelPumpNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateFuelPumpRequest.FuelPumpName), $"Fuel pump name must not exceed {MaxFuelPumpNameLength} characters."));
        }

        return result;
    }
}
