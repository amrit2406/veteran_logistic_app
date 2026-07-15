using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validator interface for delete vehicle assignment requests.
/// </summary>
public interface IDeleteVehicleAssignmentValidator
{
    /// <summary>
    /// Validates a delete vehicle assignment request.
    /// </summary>
    /// <param name="request">The delete vehicle assignment request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteVehicleAssignmentRequest request);
}
