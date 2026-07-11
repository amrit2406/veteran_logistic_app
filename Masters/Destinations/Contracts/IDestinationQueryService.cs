using veteran_logistic.Masters.Destinations.Models;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Service contract for destination query operations.
/// </summary>
public interface IDestinationQueryService
{
    /// <summary>
    /// Gets all destinations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of destination list items.</returns>
    Task<IEnumerable<DestinationListItem>> GetAllDestinationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches destinations based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of destination list items matching the search criteria.</returns>
    Task<IEnumerable<DestinationListItem>> SearchDestinationsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a destination for editing by ID.
    /// </summary>
    /// <param name="destinationId">The destination ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The destination model, or null if not found.</returns>
    Task<DestinationModel?> GetDestinationForEditAsync(int destinationId, CancellationToken cancellationToken = default);
}
