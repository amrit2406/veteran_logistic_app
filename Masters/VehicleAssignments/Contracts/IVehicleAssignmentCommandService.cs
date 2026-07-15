using veteran_logistic.Masters.VehicleAssignments.Models;

namespace veteran_logistic.Masters.VehicleAssignments.Contracts;

/// <summary>
/// Service contract for vehicle assignment command operations.
/// </summary>
public interface IVehicleAssignmentCommandService
{
    /// <summary>
    /// Assigns a vehicle to an owner.
    /// </summary>
    /// <param name="request">The vehicle assignment request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created assignment ID.</returns>
    Task<AssignVehicleResult> AssignVehicleAsync(AssignVehicleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vehicle assignment.
    /// </summary>
    /// <param name="request">The vehicle assignment update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVehicleAssignmentResult> UpdateAssignmentAsync(UpdateVehicleAssignmentRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Releases a vehicle from an owner.
    /// </summary>
    /// <param name="request">The vehicle release request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<ReleaseVehicleResult> ReleaseVehicleAsync(ReleaseVehicleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a vehicle assignment (soft delete).
    /// </summary>
    /// <param name="request">The delete assignment request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteVehicleAssignmentResult> DeleteAssignmentAsync(DeleteVehicleAssignmentRequest request, CancellationToken cancellationToken = default);
}
