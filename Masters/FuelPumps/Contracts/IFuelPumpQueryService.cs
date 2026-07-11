using veteran_logistic.Masters.FuelPumps.Models;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Service contract for fuel pump query operations.
/// </summary>
public interface IFuelPumpQueryService
{
    /// <summary>
    /// Gets all fuel pumps.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of fuel pump list items.</returns>
    Task<IReadOnlyList<FuelPumpListItem>> GetAllFuelPumpsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches fuel pumps based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to filter by.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of fuel pump list items matching the search criteria.</returns>
    Task<IReadOnlyList<FuelPumpListItem>> SearchFuelPumpsAsync(string? searchTerm, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a fuel pump for editing by ID.
    /// </summary>
    /// <param name="fuelPumpId">The fuel pump ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The fuel pump model, or null if not found.</returns>
    Task<FuelPumpModel?> GetFuelPumpForEditAsync(int fuelPumpId, CancellationToken cancellationToken = default);
}
