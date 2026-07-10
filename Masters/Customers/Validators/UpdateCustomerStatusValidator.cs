using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Validators;

/// <summary>
/// Validator for customer status update requests.
/// </summary>
public sealed class UpdateCustomerStatusValidator : IUpdateCustomerStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateCustomerStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerStatusRequest), "Update customer status request cannot be null."));
            return result;
        }

        if (request.CustomerId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerStatusRequest.CustomerId), "Customer ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Customer is already active."
                : "Customer is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateCustomerStatusRequest.IsActive), message));
        }

        return result;
    }
}
