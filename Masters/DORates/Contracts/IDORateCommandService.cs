using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Service interface for DO Rate command operations.
/// </summary>
public interface IDORateCommandService
{
    /// <summary>
    /// Creates a new DO Rate.
    /// </summary>
    /// <param name="request">The creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The creation result.</returns>
    Task<CreateDORateResult> CreateDORateAsync(CreateDORateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing DO Rate.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The update result.</returns>
    Task<UpdateDORateResult> UpdateDORateAsync(UpdateDORateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the active status of a DO Rate.
    /// </summary>
    /// <param name="request">The status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The status update result.</returns>
    Task<UpdateDORateStatusResult> UpdateDORateStatusAsync(UpdateDORateStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes (soft deletes) a DO Rate.
    /// </summary>
    /// <param name="request">The deletion request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The deletion result.</returns>
    Task<DeleteDORateResult> DeleteDORateAsync(DeleteDORateRequest request, CancellationToken cancellationToken = default);
}
