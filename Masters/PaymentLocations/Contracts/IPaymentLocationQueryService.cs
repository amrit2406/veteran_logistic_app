using veteran_logistic.Masters.PaymentLocations.Models;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Service contract for payment location query operations.
/// </summary>
public interface IPaymentLocationQueryService
{
    /// <summary>
    /// Gets all payment locations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of payment location list items.</returns>
    Task<IReadOnlyList<PaymentLocationListItem>> GetAllPaymentLocationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches payment locations based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of payment location list items matching the search criteria.</returns>
    Task<IReadOnlyList<PaymentLocationListItem>> SearchPaymentLocationsAsync(string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a payment location for editing by ID.
    /// </summary>
    /// <param name="paymentLocationId">The payment location ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment location model, or null if not found.</returns>
    Task<PaymentLocationModel?> GetPaymentLocationForEditAsync(int paymentLocationId, CancellationToken cancellationToken = default);
}
