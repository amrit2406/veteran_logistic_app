using veteran_logistic.Masters.Sources.Models;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Service contract for source query operations.
/// </summary>
public interface ISourceQueryService
{
    /// <summary>
    /// Gets all sources.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of source list items.</returns>
    Task<IEnumerable<SourceListItem>> GetAllSourcesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches sources based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of source list items matching the search criteria.</returns>
    Task<IEnumerable<SourceListItem>> SearchSourcesAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a source for editing by ID.
    /// </summary>
    /// <param name="sourceId">The source ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The source model, or null if not found.</returns>
    Task<SourceModel?> GetSourceForEditAsync(int sourceId, CancellationToken cancellationToken = default);
}
