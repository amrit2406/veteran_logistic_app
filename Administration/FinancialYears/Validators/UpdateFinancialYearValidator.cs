using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Validators;

/// <summary>
/// Validator for financial year update requests.
/// </summary>
public sealed class UpdateFinancialYearValidator : IUpdateFinancialYearValidator
{
    private const int MaxNameLength = 100;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateFinancialYearRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest), "Update financial year request cannot be null."));
            return result;
        }

        if (request.Id <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.Id), "Financial year ID must be greater than 0."));
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.Name), "Financial year name is required."));
        }
        else
        {
            var trimmedName = request.Name.Trim();
            if (trimmedName.Length == 0)
            {
                result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.Name), "Financial year name cannot be empty."));
            }
            else if (trimmedName.Length > MaxNameLength)
            {
                result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.Name), $"Financial year name must not exceed {MaxNameLength} characters."));
            }
        }

        if (request.StartDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.StartDate), "Start date is required."));
        }

        if (request.EndDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.EndDate), "End date is required."));
        }

        if (request.StartDate != default && request.EndDate != default && request.EndDate <= request.StartDate)
        {
            result.AddError(new ValidationError(nameof(UpdateFinancialYearRequest.EndDate), "End date must be greater than start date."));
        }

        return result;
    }
}
