using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Validator interface for vehicle owner status update requests.
/// </summary>
public interface IUpdateVehicleOwnerStatusValidator
{
    /// <summary>
    /// Validates the vehicle owner status update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="currentIsActive">The current active status of the vehicle owner.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(UpdateVehicleOwnerStatusRequest request, bool currentIsActive);
}
