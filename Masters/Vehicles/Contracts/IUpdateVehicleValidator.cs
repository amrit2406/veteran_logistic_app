using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Validator interface for vehicle update requests.
/// </summary>
public interface IUpdateVehicleValidator
{
    /// <summary>
    /// Validates the vehicle update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(UpdateVehicleRequest request);
}
