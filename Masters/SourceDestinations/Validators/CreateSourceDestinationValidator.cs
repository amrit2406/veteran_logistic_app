using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Validators;

/// <summary>
/// Validator for source/destination creation requests.
/// </summary>
public sealed class CreateSourceDestinationValidator : ICreateSourceDestinationValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(CreateSourceDestinationRequest request)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.LocationName))
        {
            result.AddError(new ValidationError(nameof(CreateSourceDestinationRequest.LocationName), "Source/Destination name is required."));
        }
        else if (request.LocationName.Length > 200)
        {
            result.AddError(new ValidationError(nameof(CreateSourceDestinationRequest.LocationName), "Source/Destination name cannot exceed 200 characters."));
        }

        return result;
    }
}
