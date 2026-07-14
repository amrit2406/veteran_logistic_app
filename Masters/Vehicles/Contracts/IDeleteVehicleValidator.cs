using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Validator interface for vehicle delete requests.
/// </summary>
public interface IDeleteVehicleValidator
{
    /// <summary>
    /// Validates the vehicle delete request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(DeleteVehicleRequest request);
}
