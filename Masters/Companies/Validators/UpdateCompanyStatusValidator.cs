using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Companies.Validators;

/// <summary>
/// Validator for company status update requests.
/// </summary>
public sealed class UpdateCompanyStatusValidator : IUpdateCompanyStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateCompanyStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyStatusRequest), "Update company status request cannot be null."));
            return result;
        }

        if (request.CompanyId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateCompanyStatusRequest.CompanyId), "Company ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Company is already active."
                : "Company is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateCompanyStatusRequest.IsActive), message));
        }

        return result;
    }
}
