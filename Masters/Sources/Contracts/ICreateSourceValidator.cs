using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Contract for validating create source requests.
/// </summary>
public interface ICreateSourceValidator
{
    /// <summary>
    /// Validates a create source request.
    /// </summary>
    /// <param name="request">The create source request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateSourceRequest request);
}
