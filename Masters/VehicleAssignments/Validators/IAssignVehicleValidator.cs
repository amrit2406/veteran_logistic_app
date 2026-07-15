using veteran_logistic.Masters.VehicleAssignments.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleAssignments.Validators;

/// <summary>
/// Validator interface for assign vehicle requests.
/// </summary>
public interface IAssignVehicleValidator
{
    /// <summary>
    /// Validates an assign vehicle request.
    /// </summary>
    /// <param name="request">The assign vehicle request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(AssignVehicleRequest request);
}
