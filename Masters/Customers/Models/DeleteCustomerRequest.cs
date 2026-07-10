namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Request model for deleting a customer.
/// </summary>
public sealed class DeleteCustomerRequest
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public int CustomerId { get; set; }
}
