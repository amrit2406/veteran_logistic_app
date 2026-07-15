using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validator interface for release vehicle requests.
/// </summary>
public interface IReleaseVehicleValidator
{
    /// <summary>
    /// Validates a release vehicle request.
    /// </summary>
    /// <param name="request">The release vehicle request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(ReleaseVehicleRequest request);
}
