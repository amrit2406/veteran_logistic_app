using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Validators;

/// <summary>
/// Validates create company requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateCompanyValidator : ICreateCompanyValidator
{
    private const int MinCompanyCodeLength = 2;
    private const int MaxCompanyCodeLength = 50;
    private const int MaxCompanyNameLength = 200;

    /// <summary>
    /// Validates a create company request.
    /// </summary>
    /// <param name="request">The create company request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateCompanyRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest), "Create company request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.CompanyCode))
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.CompanyCode), "Company code is required."));
        }
        else if (request.CompanyCode.Length < MinCompanyCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.CompanyCode), $"Company code must be at least {MinCompanyCodeLength} characters."));
        }
        else if (request.CompanyCode.Length > MaxCompanyCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.CompanyCode), $"Company code must not exceed {MaxCompanyCodeLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.CompanyName), "Company name is required."));
        }
        else if (request.CompanyName.Length > MaxCompanyNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.CompanyName), $"Company name must not exceed {MaxCompanyNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(CreateCompanyRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
