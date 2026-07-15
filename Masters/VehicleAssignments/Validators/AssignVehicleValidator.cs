using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validates assign vehicle requests to ensure required fields are present and valid.
/// </summary>
public sealed class AssignVehicleValidator : IAssignVehicleValidator
{
    /// <summary>
    /// Validates an assign vehicle request.
    /// </summary>
    /// <param name="request">The assign vehicle request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(AssignVehicleRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest), "Assign vehicle request cannot be null."));
            return result;
        }

        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest.VehicleId), "Vehicle ID is required."));
        }

        if (string.IsNullOrWhiteSpace(request.OwnerFirstName))
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest.OwnerFirstName), "Owner first name is required."));
        }

        if (string.IsNullOrWhiteSpace(request.OwnerLastName))
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest.OwnerLastName), "Owner last name is required."));
        }

        if (string.IsNullOrWhiteSpace(request.OwnerPanNumber))
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest.OwnerPanNumber), "Owner PAN number is required."));
        }

        if (request.AssignDate == default)
        {
            result.AddError(new ValidationError(nameof(AssignVehicleRequest.AssignDate), "Assign date is required."));
        }

        return result;
    }
}
