using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Validators;

/// <summary>
/// Validator for close financial year requests.
/// </summary>
public sealed class CloseFinancialYearValidator : ICloseFinancialYearValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(CloseFinancialYearRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CloseFinancialYearRequest), "Close financial year request cannot be null."));
            return result;
        }

        if (request.FinancialYearId <= 0)
        {
            result.AddError(new ValidationError(nameof(CloseFinancialYearRequest.FinancialYearId), "Financial year ID must be greater than 0."));
        }

        return result;
    }
}
