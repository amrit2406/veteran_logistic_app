using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Validator interface for vehicle owner delete requests.
/// </summary>
public interface IDeleteVehicleOwnerValidator
{
    /// <summary>
    /// Validates the vehicle owner delete request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(DeleteVehicleOwnerRequest request);
}
