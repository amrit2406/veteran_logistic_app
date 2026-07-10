using veteran_logistic.Masters.Customers.Models;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Service contract for customer command operations.
/// </summary>
public interface ICustomerCommandService
{
    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">The customer creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created customer ID.</returns>
    Task<CreateCustomerResult> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="request">The customer update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateCustomerResult> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a customer's active status.
    /// </summary>
    /// <param name="request">The customer status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateCustomerStatusResult> UpdateCustomerStatusAsync(UpdateCustomerStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a customer (soft delete).
    /// </summary>
    /// <param name="request">The delete customer request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteCustomerResult> DeleteCustomerAsync(DeleteCustomerRequest request, CancellationToken cancellationToken = default);
}
