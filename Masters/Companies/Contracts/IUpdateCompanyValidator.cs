using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Contracts;

/// <summary>
/// Contract for validating company update requests.
/// </summary>
public interface IUpdateCompanyValidator
{
    /// <summary>
    /// Validates the company update request.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateCompanyRequest request);
}
