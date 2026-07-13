using veteran_logistic.Masters.HsdRates.Models;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Service contract for HSD rate query operations.
/// </summary>
public interface IHsdRateQueryService
{
    /// <summary>
    /// Gets all HSD rates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of HSD rate list items.</returns>
    Task<IReadOnlyList<HsdRateListItem>> GetAllHsdRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches HSD rates based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of HSD rate list items matching the search criteria.</returns>
    Task<IReadOnlyList<HsdRateListItem>> SearchHsdRatesAsync(string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an HSD rate for editing by ID.
    /// </summary>
    /// <param name="hsdRateId">The HSD rate ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HSD rate model, or null if not found.</returns>
    Task<HsdRateModel?> GetHsdRateForEditAsync(int hsdRateId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all fuel pumps for dropdown selection.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of fuel pump dropdown items.</returns>
    Task<IReadOnlyList<FuelPumpDropdownItem>> GetFuelPumpsForDropdownAsync(CancellationToken cancellationToken = default);
}
