using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Validator interface for vehicle creation requests.
/// </summary>
public interface ICreateVehicleValidator
{
    /// <summary>
    /// Validates the vehicle creation request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>A validation result.</returns>
    ValidationResult Validate(CreateVehicleRequest request);
}
