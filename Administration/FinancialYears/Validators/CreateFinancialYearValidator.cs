using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.FinancialYears.Validators;

/// <summary>
/// Validates create financial year requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateFinancialYearValidator : ICreateFinancialYearValidator
{
    private const int MaxNameLength = 100;

    /// <summary>
    /// Validates a create financial year request.
    /// </summary>
    /// <param name="request">The create financial year request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateFinancialYearRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateFinancialYearRequest), "Create financial year request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.Name), "Financial year name is required."));
        }
        else
        {
            var trimmedName = request.Name.Trim();
            if (trimmedName.Length == 0)
            {
                result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.Name), "Financial year name cannot be empty."));
            }
            else if (trimmedName.Length > MaxNameLength)
            {
                result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.Name), $"Financial year name must not exceed {MaxNameLength} characters."));
            }
        }

        if (request.StartDate == default)
        {
            result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.StartDate), "Start date is required."));
        }

        if (request.EndDate == default)
        {
            result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.EndDate), "End date is required."));
        }

        if (request.StartDate != default && request.EndDate != default && request.EndDate <= request.StartDate)
        {
            result.AddError(new ValidationError(nameof(CreateFinancialYearRequest.EndDate), "End date must be greater than start date."));
        }

        return result;
    }
}
