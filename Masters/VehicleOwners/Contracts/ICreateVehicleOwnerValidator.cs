using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Validator interface for vehicle owner creation requests.
/// </summary>
public interface ICreateVehicleOwnerValidator
{
    /// <summary>
    /// Validates the vehicle owner creation request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(CreateVehicleOwnerRequest request);
}
