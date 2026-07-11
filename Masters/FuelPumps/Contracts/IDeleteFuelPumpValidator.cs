using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Contract for validating delete fuel pump requests.
/// </summary>
public interface IDeleteFuelPumpValidator
{
    /// <summary>
    /// Validates a delete fuel pump request.
    /// </summary>
    /// <param name="request">The delete fuel pump request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteFuelPumpRequest request);
}
