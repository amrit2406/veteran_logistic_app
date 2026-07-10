using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Validators;

/// <summary>
/// Validates delete company requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteCompanyValidator : IDeleteCompanyValidator
{
    /// <summary>
    /// Validates a delete company request.
    /// </summary>
    /// <param name="request">The delete company request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteCompanyRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteCompanyRequest), "Delete company request cannot be null."));
            return result;
        }

        if (request.CompanyId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteCompanyRequest.CompanyId), "Company ID must be a positive value."));
        }

        return result;
    }
}
