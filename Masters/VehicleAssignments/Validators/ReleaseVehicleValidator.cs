using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validates release vehicle requests to ensure required fields are present and valid.
/// </summary>
public sealed class ReleaseVehicleValidator : IReleaseVehicleValidator
{
    /// <summary>
    /// Validates a release vehicle request.
    /// </summary>
    /// <param name="request">The release vehicle request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(ReleaseVehicleRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(ReleaseVehicleRequest), "Release vehicle request cannot be null."));
            return result;
        }

        if (request.AssignmentId <= 0)
        {
            result.AddError(new ValidationError(nameof(ReleaseVehicleRequest.AssignmentId), "Assignment ID is required."));
        }

        if (request.ReleaseDate == default)
        {
            result.AddError(new ValidationError(nameof(ReleaseVehicleRequest.ReleaseDate), "Release date is required."));
        }

        return result;
    }
}
