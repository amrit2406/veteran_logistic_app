using veteran_logistic.Masters.Vehicles.Models;

namespace veteran_logistic.Masters.Vehicles.Contracts;

/// <summary>
/// Service contract for vehicle command operations.
/// </summary>
public interface IVehicleCommandService
{
    /// <summary>
    /// Creates a new vehicle.
    /// </summary>
    /// <param name="request">The vehicle creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created vehicle ID.</returns>
    Task<CreateVehicleResult> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vehicle.
    /// </summary>
    /// <param name="request">The vehicle update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVehicleResult> UpdateVehicleAsync(UpdateVehicleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a vehicle's active status.
    /// </summary>
    /// <param name="request">The vehicle status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVehicleStatusResult> UpdateVehicleStatusAsync(UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a vehicle (soft delete).
    /// </summary>
    /// <param name="request">The delete vehicle request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteVehicleResult> DeleteVehicleAsync(DeleteVehicleRequest request, CancellationToken cancellationToken = default);
}
