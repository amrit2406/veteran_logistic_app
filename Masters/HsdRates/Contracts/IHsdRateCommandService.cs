using veteran_logistic.Masters.HsdRates.Models;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Service contract for HSD rate command operations.
/// </summary>
public interface IHsdRateCommandService
{
    /// <summary>
    /// Creates a new HSD rate.
    /// </summary>
    /// <param name="request">The HSD rate creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created HSD rate ID.</returns>
    Task<CreateHsdRateResult> CreateHsdRateAsync(CreateHsdRateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing HSD rate.
    /// </summary>
    /// <param name="request">The HSD rate update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateHsdRateResult> UpdateHsdRateAsync(UpdateHsdRateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an HSD rate's active status.
    /// </summary>
    /// <param name="request">The HSD rate status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateHsdRateStatusResult> UpdateHsdRateStatusAsync(UpdateHsdRateStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an HSD rate (soft delete).
    /// </summary>
    /// <param name="request">The delete HSD rate request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteHsdRateResult> DeleteHsdRateAsync(DeleteHsdRateRequest request, CancellationToken cancellationToken = default);
}
