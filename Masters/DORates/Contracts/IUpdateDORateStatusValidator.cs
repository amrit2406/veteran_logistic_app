using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Validator interface for DO Rate status update requests.
/// </summary>
public interface IUpdateDORateStatusValidator
{
    /// <summary>
    /// Validates the status update request.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(UpdateDORateStatusRequest request);
}
