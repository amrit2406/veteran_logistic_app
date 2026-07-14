using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Validators;

/// <summary>
/// Validator for vehicle delete requests.
/// </summary>
public sealed class DeleteVehicleValidator : IDeleteVehicleValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(DeleteVehicleRequest request)
    {
        var result = new ValidationResult();

        // Validate Vehicle ID
        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleId), "Vehicle ID must be positive."));
        }

        return result;
    }
}
