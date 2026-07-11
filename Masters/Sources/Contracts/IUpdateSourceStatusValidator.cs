using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Contract for validating source status update requests.
/// </summary>
public interface IUpdateSourceStatusValidator
{
    /// <summary>
    /// Validates the source status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the source.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateSourceStatusRequest request, bool currentIsActive);
}
