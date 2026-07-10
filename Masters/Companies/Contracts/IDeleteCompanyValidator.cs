using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Contracts;

/// <summary>
/// Contract for validating delete company requests.
/// </summary>
public interface IDeleteCompanyValidator
{
    /// <summary>
    /// Validates a delete company request.
    /// </summary>
    /// <param name="request">The delete company request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteCompanyRequest request);
}
