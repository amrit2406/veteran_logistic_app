using veteran_logistic.Masters.VehicleOwners.Models;

namespace veteran_logistic.Masters.VehicleOwners.Contracts;

/// <summary>
/// Service contract for vehicle owner command operations.
/// </summary>
public interface IVehicleOwnerCommandService
{
    /// <summary>
    /// Creates a new vehicle owner.
    /// </summary>
    /// <param name="request">The vehicle owner creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created vehicle owner ID.</returns>
    Task<CreateVehicleOwnerResult> CreateVehicleOwnerAsync(CreateVehicleOwnerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vehicle owner.
    /// </summary>
    /// <param name="request">The vehicle owner update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVehicleOwnerResult> UpdateVehicleOwnerAsync(UpdateVehicleOwnerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a vehicle owner's active status.
    /// </summary>
    /// <param name="request">The vehicle owner status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVehicleOwnerStatusResult> UpdateVehicleOwnerStatusAsync(UpdateVehicleOwnerStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a vehicle owner (soft delete).
    /// </summary>
    /// <param name="request">The delete vehicle owner request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteVehicleOwnerResult> DeleteVehicleOwnerAsync(DeleteVehicleOwnerRequest request, CancellationToken cancellationToken = default);
}
