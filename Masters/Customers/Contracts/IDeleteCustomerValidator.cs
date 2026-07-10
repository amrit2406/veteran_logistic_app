using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Contract for validating delete customer requests.
/// </summary>
public interface IDeleteCustomerValidator
{
    /// <summary>
    /// Validates a delete customer request.
    /// </summary>
    /// <param name="request">The delete customer request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteCustomerRequest request);
}
