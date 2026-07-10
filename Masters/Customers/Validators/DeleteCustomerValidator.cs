using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Validators;

/// <summary>
/// Validates delete customer requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteCustomerValidator : IDeleteCustomerValidator
{
    /// <summary>
    /// Validates a delete customer request.
    /// </summary>
    /// <param name="request">The delete customer request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteCustomerRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteCustomerRequest), "Delete customer request cannot be null."));
            return result;
        }

        if (request.CustomerId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteCustomerRequest.CustomerId), "Customer ID must be a positive value."));
        }

        return result;
    }
}
