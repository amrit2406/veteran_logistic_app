using veteran_logistic.Masters.Vehicles.Models;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Service contract for querying vehicle data.
/// </summary>
public interface IVehicleQueryService
{
    /// <summary>
    /// Gets all vehicles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle list items.</returns>
    Task<IReadOnlyList<VehicleListItem>> GetAllVehiclesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches vehicles by vehicle number, vehicle type, owner code, or owner name.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle list items matching the search criteria.</returns>
    Task<IReadOnlyList<VehicleListItem>> SearchVehiclesAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a vehicle for editing by vehicle ID.
    /// </summary>
    /// <param name="id">The vehicle ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle model, or null if not found.</returns>
    Task<VehicleModel?> GetVehicleForEditAsync(int id, CancellationToken cancellationToken = default);
}
