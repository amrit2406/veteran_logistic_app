using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Validator interface for vehicle owner update requests.
/// </summary>
public interface IUpdateVehicleOwnerValidator
{
    /// <summary>
    /// Validates the vehicle owner update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(UpdateVehicleOwnerRequest request);
}
