using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Validators;

/// <summary>
/// Validates delete source requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteSourceValidator : IDeleteSourceValidator
{
    /// <summary>
    /// Validates a delete source request.
    /// </summary>
    /// <param name="request">The delete source request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteSourceRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteSourceRequest), "Delete source request cannot be null."));
            return result;
        }

        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteSourceRequest.SourceId), "Source ID must be a positive value."));
        }

        return result;
    }
}
