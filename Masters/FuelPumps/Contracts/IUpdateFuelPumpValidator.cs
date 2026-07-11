using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Contract for validating update fuel pump requests.
/// </summary>
public interface IUpdateFuelPumpValidator
{
    /// <summary>
    /// Validates an update fuel pump request.
    /// </summary>
    /// <param name="request">The update fuel pump request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateFuelPumpRequest request);
}
