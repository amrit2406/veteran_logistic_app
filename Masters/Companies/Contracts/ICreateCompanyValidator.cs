using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Contracts;

/// <summary>
/// Contract for validating create company requests.
/// </summary>
public interface ICreateCompanyValidator
{
    /// <summary>
    /// Validates a create company request.
    /// </summary>
    /// <param name="request">The create company request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateCompanyRequest request);
}
