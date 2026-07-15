using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validates update vehicle assignment requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdateVehicleAssignmentValidator : IUpdateVehicleAssignmentValidator
{
    /// <summary>
    /// Validates an update vehicle assignment request.
    /// </summary>
    /// <param name="request">The update vehicle assignment request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdateVehicleAssignmentRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateVehicleAssignmentRequest), "Update vehicle assignment request cannot be null."));
            return result;
        }

        if (request.AssignmentId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateVehicleAssignmentRequest.AssignmentId), "Assignment ID is required."));
        }

        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateVehicleAssignmentRequest.VehicleId), "Vehicle ID is required."));
        }

        if (request.VehicleOwnerId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateVehicleAssignmentRequest.VehicleOwnerId), "Vehicle owner ID is required."));
        }

        if (request.AssignDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateVehicleAssignmentRequest.AssignDate), "Assign date is required."));
        }

        return result;
    }
}
