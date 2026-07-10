using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Validators;

/// <summary>
/// Validates create customer requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateCustomerValidator : ICreateCustomerValidator
{
    private const int MinCustomerCodeLength = 2;
    private const int MaxCustomerCodeLength = 50;
    private const int MaxCustomerNameLength = 200;

    /// <summary>
    /// Validates a create customer request.
    /// </summary>
    /// <param name="request">The create customer request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateCustomerRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest), "Create customer request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.CustomerCode))
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.CustomerCode), "Customer code is required."));
        }
        else if (request.CustomerCode.Length < MinCustomerCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.CustomerCode), $"Customer code must be at least {MinCustomerCodeLength} characters."));
        }
        else if (request.CustomerCode.Length > MaxCustomerCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.CustomerCode), $"Customer code must not exceed {MaxCustomerCodeLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.CustomerName), "Customer name is required."));
        }
        else if (request.CustomerName.Length > MaxCustomerNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.CustomerName), $"Customer name must not exceed {MaxCustomerNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(CreateCustomerRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
