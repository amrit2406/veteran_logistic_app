using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Service interface for querying DO Rate data.
/// </summary>
public interface IDORateQueryService
{
    /// <summary>
    /// Gets all DO rates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of DO rate list items.</returns>
    Task<IEnumerable<DORateListItem>> GetAllDORatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches DO rates by various criteria.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of matching DO rate list items.</returns>
    Task<IEnumerable<DORateListItem>> SearchDORatesAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a DO rate for editing.
    /// </summary>
    /// <param name="id">The DO rate ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The DO rate model, or null if not found.</returns>
    Task<DORateModel?> GetDORateForEditAsync(int id, CancellationToken cancellationToken = default);
}
