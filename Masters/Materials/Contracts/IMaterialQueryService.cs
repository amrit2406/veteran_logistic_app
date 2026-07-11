using veteran_logistic.Masters.Materials.Models;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Service contract for material query operations.
/// </summary>
public interface IMaterialQueryService
{
    /// <summary>
    /// Gets all materials.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of material list items.</returns>
    Task<IReadOnlyList<MaterialListItem>> GetAllMaterialsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches materials based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of material list items matching the search criteria.</returns>
    Task<IReadOnlyList<MaterialListItem>> SearchMaterialsAsync(string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a material for editing by ID.
    /// </summary>
    /// <param name="materialId">The material ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The material model, or null if not found.</returns>
    Task<MaterialModel?> GetMaterialForEditAsync(int materialId, CancellationToken cancellationToken = default);
}
