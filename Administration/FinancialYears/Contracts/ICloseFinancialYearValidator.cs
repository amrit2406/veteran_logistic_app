using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Contract for validating close financial year requests.
/// </summary>
public interface ICloseFinancialYearValidator
{
    /// <summary>
    /// Validates the close financial year request.
    /// </summary>
    /// <param name="request">The close financial year request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(CloseFinancialYearRequest request);
}
