using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Contract for validating create fuel pump requests.
/// </summary>
public interface ICreateFuelPumpValidator
{
    /// <summary>
    /// Validates a create fuel pump request.
    /// </summary>
    /// <param name="request">The create fuel pump request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateFuelPumpRequest request);
}
