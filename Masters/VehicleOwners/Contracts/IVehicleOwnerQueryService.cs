using veteran_logistic.Masters.VehicleOwners.Models;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Service contract for querying vehicle owner data.
/// </summary>
public interface IVehicleOwnerQueryService
{
    /// <summary>
    /// Gets all vehicle owners.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle owner list items.</returns>
    Task<IReadOnlyList<VehicleOwnerListItem>> GetAllVehicleOwnersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches vehicle owners by owner code, first name, last name, company name, PAN number, mobile, or city.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle owner list items matching the search criteria.</returns>
    Task<IReadOnlyList<VehicleOwnerListItem>> SearchVehicleOwnersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a vehicle owner for editing by vehicle owner ID.
    /// </summary>
    /// <param name="id">The vehicle owner ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle owner model, or null if not found.</returns>
    Task<VehicleOwnerModel?> GetVehicleOwnerForEditAsync(int id, CancellationToken cancellationToken = default);
}
