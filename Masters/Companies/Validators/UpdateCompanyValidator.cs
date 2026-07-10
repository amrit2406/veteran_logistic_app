using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Validators;

/// <summary>
/// Validator for company update requests.
/// </summary>
public sealed class UpdateCompanyValidator : IUpdateCompanyValidator
{
    private const int MaxCompanyNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateCompanyRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest), "Update company request cannot be null."));
            return result;
        }

        if (request.CompanyId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest.CompanyId), "Company ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.CompanyCode))
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest.CompanyCode), "Company code is required."));
        }

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest.CompanyName), "Company name is required."));
        }
        else if (request.CompanyName.Length > MaxCompanyNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest.CompanyName), $"Company name must not exceed {MaxCompanyNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
