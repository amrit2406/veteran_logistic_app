using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Validators;

/// <summary>
/// Validates delete financial year requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteFinancialYearValidator : IDeleteFinancialYearValidator
{
    /// <summary>
    /// Validates a delete financial year request.
    /// </summary>
    /// <param name="request">The delete financial year request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteFinancialYearRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteFinancialYearRequest), "Delete financial year request cannot be null."));
            return result;
        }

        if (request.FinancialYearId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteFinancialYearRequest.FinancialYearId), "Financial year ID must be a positive value."));
        }

        return result;
    }
}
