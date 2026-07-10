using veteran_logistic.Masters.Customers.Models;

namespace veteran_logistic.Masters.Customers.Contracts;

/// <summary>
/// Service contract for querying customer data.
/// </summary>
public interface ICustomerQueryService
{
    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of customer list items.</returns>
    Task<IReadOnlyList<CustomerListItem>> GetAllCustomersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches customers by customer code, customer name, city, or state.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of customer list items matching the search criteria.</returns>
    Task<IReadOnlyList<CustomerListItem>> SearchCustomersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a customer for editing by customer ID.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The customer model, or null if not found.</returns>
    Task<CustomerModel?> GetCustomerForEditAsync(int id, CancellationToken cancellationToken = default);
}
