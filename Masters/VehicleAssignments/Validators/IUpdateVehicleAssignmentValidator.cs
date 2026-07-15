using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validator interface for update vehicle assignment requests.
/// </summary>
public interface IUpdateVehicleAssignmentValidator
{
    /// <summary>
    /// Validates an update vehicle assignment request.
    /// </summary>
    /// <param name="request">The update vehicle assignment request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateVehicleAssignmentRequest request);
}
