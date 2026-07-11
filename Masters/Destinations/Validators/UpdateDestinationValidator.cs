using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Validators;

/// <summary>
/// Validator for destination update requests.
/// </summary>
public sealed class UpdateDestinationValidator : IUpdateDestinationValidator
{
    private const int MaxDestinationNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateDestinationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationRequest), "Update destination request cannot be null."));
            return result;
        }

        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationRequest.DestinationId), "Destination ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.DestinationCode))
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationRequest.DestinationCode), "Destination code is required."));
        }

        if (string.IsNullOrWhiteSpace(request.DestinationName))
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationRequest.DestinationName), "Destination name is required."));
        }
        else if (request.DestinationName.Length > MaxDestinationNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateDestinationRequest.DestinationName), $"Destination name must not exceed {MaxDestinationNameLength} characters."));
        }

        return result;
    }
}
