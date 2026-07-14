using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Validator interface for vehicle status update requests.
/// </summary>
public interface IUpdateVehicleStatusValidator
{
    /// <summary>
    /// Validates the vehicle status update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="currentIsActive">The current active status of the vehicle.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(UpdateVehicleStatusRequest request, bool currentIsActive);
}
