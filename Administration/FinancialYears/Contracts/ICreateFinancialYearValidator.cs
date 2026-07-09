using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Contract for validating create financial year requests.
/// </summary>
public interface ICreateFinancialYearValidator
{
    /// <summary>
    /// Validates a create financial year request.
    /// </summary>
    /// <param name="request">The create financial year request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateFinancialYearRequest request);
}
