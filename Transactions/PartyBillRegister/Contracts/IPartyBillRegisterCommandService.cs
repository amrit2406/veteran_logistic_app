using veteran_logistic.Transactions.PartyBillRegister.Models;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Service contract for party bill register command operations.
/// </summary>
public interface IPartyBillRegisterCommandService
{
    /// <summary>
    /// Creates a new party bill register.
    /// </summary>
    /// <param name="request">The party bill register creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created party bill register ID and bill number.</returns>
    Task<CreatePartyBillRegisterResult> CreatePartyBillRegisterAsync(CreatePartyBillRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing party bill register.
    /// </summary>
    /// <param name="request">The party bill register update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePartyBillRegisterResult> UpdatePartyBillRegisterAsync(UpdatePartyBillRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a party bill register's active status.
    /// </summary>
    /// <param name="request">The party bill register status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePartyBillRegisterStatusResult> UpdatePartyBillRegisterStatusAsync(UpdatePartyBillRegisterStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a party bill register (soft delete).
    /// </summary>
    /// <param name="request">The delete party bill register request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeletePartyBillRegisterResult> DeletePartyBillRegisterAsync(DeletePartyBillRegisterRequest request, CancellationToken cancellationToken = default);
}
