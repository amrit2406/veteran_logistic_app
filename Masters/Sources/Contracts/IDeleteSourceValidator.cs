using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Contract for validating delete source requests.
/// </summary>
public interface IDeleteSourceValidator
{
    /// <summary>
    /// Validates a delete source request.
    /// </summary>
    /// <param name="request">The delete source request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteSourceRequest request);
}
