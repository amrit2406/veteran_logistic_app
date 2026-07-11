using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Validators;

/// <summary>
/// Validator for source update requests.
/// </summary>
public sealed class UpdateSourceValidator : IUpdateSourceValidator
{
    private const int MaxSourceNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateSourceRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceRequest), "Update source request cannot be null."));
            return result;
        }

        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceRequest.SourceId), "Source ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.SourceCode))
        {
            result.AddError(new ValidationError(nameof(UpdateSourceRequest.SourceCode), "Source code is required."));
        }

        if (string.IsNullOrWhiteSpace(request.SourceName))
        {
            result.AddError(new ValidationError(nameof(UpdateSourceRequest.SourceName), "Source name is required."));
        }
        else if (request.SourceName.Length > MaxSourceNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceRequest.SourceName), $"Source name must not exceed {MaxSourceNameLength} characters."));
        }

        return result;
    }
}
