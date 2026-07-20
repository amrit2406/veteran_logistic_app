using veteran_logistic.Masters.SourceDestinations.Models;

namespace veteran_logistic.Masters.SourceDestinations.Contracts;

/// <summary>
/// Service interface for source/destination command operations.
/// </summary>
public interface ISourceDestinationCommandService
{
    /// <summary>
    /// Creates a new source/destination.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The creation result.</returns>
    Task<CreateSourceDestinationResult> CreateSourceDestinationAsync(CreateSourceDestinationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing source/destination.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The update result.</returns>
    Task<UpdateSourceDestinationResult> UpdateSourceDestinationAsync(UpdateSourceDestinationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the active status of a source/destination.
    /// </summary>
    /// <param name="request">The status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The status update result.</returns>
    Task<UpdateSourceDestinationStatusResult> UpdateSourceDestinationStatusAsync(UpdateSourceDestinationStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes (soft deletes) a source/destination.
    /// </summary>
    /// <param name="request">The deletion request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The deletion result.</returns>
    Task<DeleteSourceDestinationResult> DeleteSourceDestinationAsync(DeleteSourceDestinationRequest request, CancellationToken cancellationToken = default);
}
