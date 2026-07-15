using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validates delete vehicle assignment requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteVehicleAssignmentValidator : IDeleteVehicleAssignmentValidator
{
    /// <summary>
    /// Validates a delete vehicle assignment request.
    /// </summary>
    /// <param name="request">The delete vehicle assignment request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteVehicleAssignmentRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteVehicleAssignmentRequest), "Delete vehicle assignment request cannot be null."));
            return result;
        }

        if (request.AssignmentId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteVehicleAssignmentRequest.AssignmentId), "Assignment ID is required."));
        }

        return result;
    }
}
