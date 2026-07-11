using veteran_logistic.Masters.Sources.Models;

namespace veteran_logistic.Masters.Sources.Contracts;

/// <summary>
/// Service contract for source command operations.
/// </summary>
public interface ISourceCommandService
{
    /// <summary>
    /// Creates a new source.
    /// </summary>
    /// <param name="request">The source creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created source ID.</returns>
    Task<CreateSourceResult> CreateSourceAsync(CreateSourceRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing source.
    /// </summary>
    /// <param name="request">The source update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateSourceResult> UpdateSourceAsync(UpdateSourceRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a source's active status.
    /// </summary>
    /// <param name="request">The source status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateSourceStatusResult> UpdateSourceStatusAsync(UpdateSourceStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a source (soft delete).
    /// </summary>
    /// <param name="request">The delete source request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteSourceResult> DeleteSourceAsync(DeleteSourceRequest request, CancellationToken cancellationToken = default);
}
