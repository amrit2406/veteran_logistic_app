using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Validators;

/// <summary>
/// Validates create source requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateSourceValidator : ICreateSourceValidator
{
    private const int MinSourceCodeLength = 2;
    private const int MaxSourceCodeLength = 50;
    private const int MaxSourceNameLength = 200;

    /// <summary>
    /// Validates a create source request.
    /// </summary>
    /// <param name="request">The create source request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateSourceRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest), "Create source request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.SourceCode))
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest.SourceCode), "Source code is required."));
        }
        else if (request.SourceCode.Length < MinSourceCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest.SourceCode), $"Source code must be at least {MinSourceCodeLength} characters."));
        }
        else if (request.SourceCode.Length > MaxSourceCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest.SourceCode), $"Source code must not exceed {MaxSourceCodeLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.SourceName))
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest.SourceName), "Source name is required."));
        }
        else if (request.SourceName.Length > MaxSourceNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateSourceRequest.SourceName), $"Source name must not exceed {MaxSourceNameLength} characters."));
        }

        return result;
    }
}
