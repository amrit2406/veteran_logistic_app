using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Contract for validating customer update requests.
/// </summary>
public interface IUpdateCustomerValidator
{
    /// <summary>
    /// Validates the customer update request.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateCustomerRequest request);
}
