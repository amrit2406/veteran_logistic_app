using veteran_logistic.Masters.VehicleAssignments.Models;

namespace veteran_logistic.Masters.VehicleAssignments.Contracts;

/// <summary>
/// Service contract for querying vehicle assignment data.
/// </summary>
public interface IVehicleAssignmentQueryService
{
    /// <summary>
    /// Gets all vehicle assignments.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle assignment list items.</returns>
    Task<IReadOnlyList<VehicleAssignmentListItem>> GetAllAssignmentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches vehicle assignments by vehicle number, owner name, or PAN number.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vehicle assignment list items matching the search criteria.</returns>
    Task<IReadOnlyList<VehicleAssignmentListItem>> SearchAssignmentsAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a vehicle assignment for editing by assignment ID.
    /// </summary>
    /// <param name="id">The assignment ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vehicle assignment model, or null if not found.</returns>
    Task<VehicleAssignmentModel?> GetAssignmentForEditAsync(int id, CancellationToken cancellationToken = default);
}
