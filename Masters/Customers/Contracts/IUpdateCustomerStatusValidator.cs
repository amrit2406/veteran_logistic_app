using veteran_logistic.Masters.Customers.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Contract for validating customer status update requests.
/// </summary>
public interface IUpdateCustomerStatusValidator
{
    /// <summary>
    /// Validates the customer status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the customer.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateCustomerStatusRequest request, bool currentIsActive);
}
