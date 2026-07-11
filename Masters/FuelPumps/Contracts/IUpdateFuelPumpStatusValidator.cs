using veteran_logistic.Masters.FuelPumps.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Contract for validating fuel pump status update requests.
/// </summary>
public interface IUpdateFuelPumpStatusValidator
{
    /// <summary>
    /// Validates the fuel pump status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the fuel pump.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateFuelPumpStatusRequest request, bool currentIsActive);
}
