using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Validators;

/// <summary>
/// Validator for customer update requests.
/// </summary>
public sealed class UpdateCustomerValidator : IUpdateCustomerValidator
{
    private const int MaxCustomerNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateCustomerRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest), "Update customer request cannot be null."));
            return result;
        }

        if (request.CustomerId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest.CustomerId), "Customer ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.CustomerCode))
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest.CustomerCode), "Customer code is required."));
        }

        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest.CustomerName), "Customer name is required."));
        }
        else if (request.CustomerName.Length > MaxCustomerNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest.CustomerName), $"Customer name must not exceed {MaxCustomerNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(UpdateCustomerRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
