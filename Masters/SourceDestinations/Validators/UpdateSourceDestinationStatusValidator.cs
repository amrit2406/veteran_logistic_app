using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Validators;

/// <summary>
/// Validator for source/destination status update requests.
/// </summary>
public sealed class UpdateSourceDestinationStatusValidator : IUpdateSourceDestinationStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateSourceDestinationStatusRequest request)
    {
        var result = new ValidationResult();

        if (request.SourceDestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceDestinationStatusRequest.SourceDestinationId), "Source/Destination ID must be positive."));
        }

        return result;
    }
}
