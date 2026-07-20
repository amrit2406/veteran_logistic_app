using veteran_logistic.Masters.SourceDestinations.Models;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Service interface for querying source/destination data.
/// </summary>
public interface ISourceDestinationQueryService
{
    /// <summary>
    /// Gets all source/destinations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of source/destination list items.</returns>
    Task<IEnumerable<SourceDestinationListItem>> GetAllSourceDestinationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches source/destinations by name.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of matching source/destination list items.</returns>
    Task<IEnumerable<SourceDestinationListItem>> SearchSourceDestinationsAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a source/destination for editing.
    /// </summary>
    /// <param name="id">The source/destination ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The source/destination model, or null if not found.</returns>
    Task<SourceDestinationModel?> GetSourceDestinationForEditAsync(int id, CancellationToken cancellationToken = default);
}
