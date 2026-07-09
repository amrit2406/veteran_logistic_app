using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Contract for validating set current financial year requests.
/// </summary>
public interface ISetCurrentFinancialYearValidator
{
    /// <summary>
    /// Validates the set current financial year request.
    /// </summary>
    /// <param name="request">The set current financial year request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(SetCurrentFinancialYearRequest request);
}
