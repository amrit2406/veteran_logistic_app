using veteran_logistic.Masters.VehicleAssignments.Models;

namespace veteran_logistic.Masters.VehicleAssignments.Contracts;

/// <summary>
/// Service contract for querying vehicle assignment data.
/// </summary>
public interface IVehicleAssignmentQueryService
{
    /// <summary>
    /// Gets vehicle assignment details by vehicle number.
    /// </summary>
    /// <param name="vehicleNumber">The vehicle number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle assignment model, or null if not found.</returns>
    Task<VehicleAssignmentModel?> GetVehicleAssignmentAsync(string vehicleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches vehicles by vehicle number.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle assignment models matching the search criteria.</returns>
    Task<IReadOnlyList<VehicleAssignmentModel>> SearchVehiclesAsync(string? search, CancellationToken cancellationToken = default);
}
