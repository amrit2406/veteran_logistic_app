using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Service for providing lookup data for Consignors and Consignees.
/// This service uses the Customer entity to provide real customer data.
/// </summary>
public interface IDummyLookupService
{
    /// <summary>
    /// Gets the collection of Consignors (Customers).
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of Consignor lookup items.</returns>
    Task<IEnumerable<LookupItem>> GetConsignorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the collection of Consignees (Customers).
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The collection of Consignee lookup items.</returns>
    Task<IEnumerable<LookupItem>> GetConsigneesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the name of a Consignor by ID.
    /// </summary>
    /// <param name="consignorId">The Consignor ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Consignor name, or empty string if not found.</returns>
    Task<string> GetConsignorNameAsync(int consignorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the name of a Consignee by ID.
    /// </summary>
    /// <param name="consigneeId">The Consignee ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Consignee name, or empty string if not found.</returns>
    Task<string> GetConsigneeNameAsync(int consigneeId, CancellationToken cancellationToken = default);
}
