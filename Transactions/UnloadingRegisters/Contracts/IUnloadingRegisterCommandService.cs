using veteran_logistic.Transactions.UnloadingRegisters.Models;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Service contract for unloading register command operations.
/// </summary>
public interface IUnloadingRegisterCommandService
{
    /// <summary>
    /// Creates a new unloading register.
    /// </summary>
    /// <param name="request">The unloading register creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created unloading register ID.</returns>
    Task<CreateUnloadingRegisterResult> CreateUnloadingRegisterAsync(CreateUnloadingRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing unloading register.
    /// </summary>
    /// <param name="request">The unloading register update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateUnloadingRegisterResult> UpdateUnloadingRegisterAsync(UpdateUnloadingRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an unloading register's active status.
    /// </summary>
    /// <param name="request">The unloading register status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateUnloadingRegisterStatusResult> UpdateUnloadingRegisterStatusAsync(UpdateUnloadingRegisterStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an unloading register (soft delete).
    /// </summary>
    /// <param name="request">The delete unloading register request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteUnloadingRegisterResult> DeleteUnloadingRegisterAsync(DeleteUnloadingRegisterRequest request, CancellationToken cancellationToken = default);
}
