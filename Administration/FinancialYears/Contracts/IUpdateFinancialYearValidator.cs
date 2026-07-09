using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Contract for validating financial year update requests.
/// </summary>
public interface IUpdateFinancialYearValidator
{
    /// <summary>
    /// Validates the financial year update request.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateFinancialYearRequest request);
}
