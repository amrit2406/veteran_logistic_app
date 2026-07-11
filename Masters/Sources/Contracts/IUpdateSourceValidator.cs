using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Contract for validating update source requests.
/// </summary>
public interface IUpdateSourceValidator
{
    /// <summary>
    /// Validates an update source request.
    /// </summary>
    /// <param name="request">The update source request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateSourceRequest request);
}
