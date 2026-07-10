namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Request model for updating a customer's active status.
/// </summary>
public sealed class UpdateCustomerStatusRequest
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
