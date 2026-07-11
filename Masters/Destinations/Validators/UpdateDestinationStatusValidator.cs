using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Validators;

/// <summary>
/// Validator for destination status update requests.
/// </summary>
public sealed class UpdateDestinationStatusValidator : IUpdateDestinationStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateDestinationStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationStatusRequest), "Update destination status request cannot be null."));
            return result;
        }

        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationStatusRequest.DestinationId), "Destination ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Destination is already active."
                : "Destination is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateDestinationStatusRequest.IsActive), message));
        }

        return result;
    }
}
