using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vehicles.Validators;

/// <summary>
/// Validator for vehicle update requests.
/// </summary>
public sealed class UpdateVehicleValidator : IUpdateVehicleValidator
{
    private static readonly string[] ValidVehicleTypes =
    [
        "10 Wheels",
        "12 Wheels",
        "14 Wheels",
        "16 Wheels",
        "18 Wheels",
        "20 Wheels",
        "22 Wheels"
    ];

    /// <inheritdoc />
    public ValidationResult Validate(UpdateVehicleRequest request)
    {
        var result = new ValidationResult();

        // Validate Vehicle ID
        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleId), "Vehicle ID must be positive."));
        }

        // Validate Vehicle Owner
        if (request.VehicleOwnerId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleOwnerId), "Vehicle Owner is required."));
        }

        // Validate Vehicle Number
        if (string.IsNullOrWhiteSpace(request.VehicleNumber))
        {
            result.AddError(new ValidationError(nameof(request.VehicleNumber), "Vehicle Number is required."));
        }
        else if (request.VehicleNumber.Length > 30)
        {
            result.AddError(new ValidationError(nameof(request.VehicleNumber), "Vehicle Number cannot exceed 30 characters."));
        }

        // Validate Vehicle Type
        if (string.IsNullOrWhiteSpace(request.VehicleType))
        {
            result.AddError(new ValidationError(nameof(request.VehicleType), "Vehicle Type is required."));
        }
        else if (!ValidVehicleTypes.Contains(request.VehicleType))
        {
            result.AddError(new ValidationError(nameof(request.VehicleType), "Vehicle Type must be one of: 10 Wheels, 12 Wheels, 14 Wheels, 16 Wheels, 18 Wheels, 20 Wheels, 22 Wheels."));
        }

        return result;
    }
}
