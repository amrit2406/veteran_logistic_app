using veteran_logistic.Masters.Destinations.Models;

namespace veteran_logistic.Masters.Destinations.Contracts;

/// <summary>
/// Service contract for destination command operations.
/// </summary>
public interface IDestinationCommandService
{
    /// <summary>
    /// Creates a new destination.
    /// </summary>
    /// <param name="request">The destination creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created destination ID.</returns>
    Task<CreateDestinationResult> CreateDestinationAsync(CreateDestinationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing destination.
    /// </summary>
    /// <param name="request">The destination update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateDestinationResult> UpdateDestinationAsync(UpdateDestinationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a destination's active status.
    /// </summary>
    /// <param name="request">The destination status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateDestinationStatusResult> UpdateDestinationStatusAsync(UpdateDestinationStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a destination (soft delete).
    /// </summary>
    /// <param name="request">The delete destination request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteDestinationResult> DeleteDestinationAsync(DeleteDestinationRequest request, CancellationToken cancellationToken = default);
}
