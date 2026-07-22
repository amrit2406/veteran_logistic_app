using veteran_logistic.Transactions.LoadingRegisters.Models;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Service contract for loading register command operations.
/// </summary>
public interface ILoadingRegisterCommandService
{
    /// <summary>
    /// Creates a new loading register.
    /// </summary>
    /// <param name="request">The loading register creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created loading register ID.</returns>
    Task<CreateLoadingRegisterResult> CreateLoadingRegisterAsync(CreateLoadingRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing loading register.
    /// </summary>
    /// <param name="request">The loading register update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateLoadingRegisterResult> UpdateLoadingRegisterAsync(UpdateLoadingRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a loading register's active status.
    /// </summary>
    /// <param name="request">The loading register status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateLoadingRegisterStatusResult> UpdateLoadingRegisterStatusAsync(UpdateLoadingRegisterStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a loading register (soft delete).
    /// </summary>
    /// <param name="request">The delete loading register request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteLoadingRegisterResult> DeleteLoadingRegisterAsync(DeleteLoadingRegisterRequest request, CancellationToken cancellationToken = default);
}
