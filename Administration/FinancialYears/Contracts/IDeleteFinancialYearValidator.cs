using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Contract for validating delete financial year requests.
/// </summary>
public interface IDeleteFinancialYearValidator
{
    /// <summary>
    /// Validates a delete financial year request.
    /// </summary>
    /// <param name="request">The delete financial year request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteFinancialYearRequest request);
}
