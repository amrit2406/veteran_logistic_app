using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Validators;

/// <summary>
/// Validator for source/destination deletion requests.
/// </summary>
public sealed class DeleteSourceDestinationValidator : IDeleteSourceDestinationValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(DeleteSourceDestinationRequest request)
    {
        var result = new ValidationResult();

        if (request.SourceDestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteSourceDestinationRequest.SourceDestinationId), "Source/Destination ID must be positive."));
        }

        return result;
    }
}
