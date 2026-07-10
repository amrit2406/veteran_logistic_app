using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Contracts;

/// <summary>
/// Contract for validating company status update requests.
/// </summary>
public interface IUpdateCompanyStatusValidator
{
    /// <summary>
    /// Validates the company status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the company.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateCompanyStatusRequest request, bool currentIsActive);
}
