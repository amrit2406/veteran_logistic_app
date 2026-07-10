using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Contract for validating create customer requests.
/// </summary>
public interface ICreateCustomerValidator
{
    /// <summary>
    /// Validates a create customer request.
    /// </summary>
    /// <param name="request">The create customer request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateCustomerRequest request);
}
