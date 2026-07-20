using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Validators;

/// <summary>
/// Validator for source/destination update requests.
/// </summary>
public sealed class UpdateSourceDestinationValidator : IUpdateSourceDestinationValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateSourceDestinationRequest request)
    {
        var result = new ValidationResult();

        if (request.SourceDestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceDestinationRequest.SourceDestinationId), "Source/Destination ID must be positive."));
        }

        if (string.IsNullOrWhiteSpace(request.LocationName))
        {
            result.AddError(new ValidationError(nameof(UpdateSourceDestinationRequest.LocationName), "Source/Destination name is required."));
        }
        else if (request.LocationName.Length > 200)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceDestinationRequest.LocationName), "Source/Destination name cannot exceed 200 characters."));
        }

        return result;
    }
}
