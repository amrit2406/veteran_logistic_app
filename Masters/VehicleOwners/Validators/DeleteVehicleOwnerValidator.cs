using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Validators;

/// <summary>
/// Validator for vehicle owner delete requests.
/// </summary>
public sealed class DeleteVehicleOwnerValidator : IDeleteVehicleOwnerValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(DeleteVehicleOwnerRequest request)
    {
        var result = new ValidationResult();

        // Validate Vehicle Owner ID
        if (request.VehicleOwnerId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleOwnerId), "Vehicle Owner ID must be positive."));
        }

        return result;
    }
}
