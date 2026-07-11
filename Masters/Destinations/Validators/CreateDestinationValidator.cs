using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Destinations.Validators;

/// <summary>
/// Validates create destination requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateDestinationValidator : ICreateDestinationValidator
{
    private const int MinDestinationCodeLength = 2;
    private const int MaxDestinationCodeLength = 50;
    private const int MaxDestinationNameLength = 200;

    /// <summary>
    /// Validates a create destination request.
    /// </summary>
    /// <param name="request">The create destination request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateDestinationRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest), "Create destination request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.DestinationCode))
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest.DestinationCode), "Destination code is required."));
        }
        else if (request.DestinationCode.Length < MinDestinationCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest.DestinationCode), $"Destination code must be at least {MinDestinationCodeLength} characters."));
        }
        else if (request.DestinationCode.Length > MaxDestinationCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest.DestinationCode), $"Destination code must not exceed {MaxDestinationCodeLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.DestinationName))
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest.DestinationName), "Destination name is required."));
        }
        else if (request.DestinationName.Length > MaxDestinationNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateDestinationRequest.DestinationName), $"Destination name must not exceed {MaxDestinationNameLength} characters."));
        }

        return result;
    }
}
