using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Validators;

/// <summary>
/// Validates delete fuel pump requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteFuelPumpValidator : IDeleteFuelPumpValidator
{
    /// <summary>
    /// Validates a delete fuel pump request.
    /// </summary>
    /// <param name="request">The delete fuel pump request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteFuelPumpRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteFuelPumpRequest), "Delete fuel pump request cannot be null."));
            return result;
        }

        if (request.FuelPumpId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteFuelPumpRequest.FuelPumpId), "Fuel pump ID must be a positive value."));
        }

        return result;
    }
}
