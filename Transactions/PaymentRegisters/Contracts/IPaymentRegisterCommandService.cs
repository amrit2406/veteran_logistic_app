using veteran_logistic.Transactions.PaymentRegisters.Models;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Service contract for payment register command operations.
/// </summary>
public interface IPaymentRegisterCommandService
{
    /// <summary>
    /// Creates a new payment register.
    /// </summary>
    /// <param name="request">The payment register creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created payment register ID.</returns>
    Task<CreatePaymentRegisterResult> CreatePaymentRegisterAsync(CreatePaymentRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment register.
    /// </summary>
    /// <param name="request">The payment register update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePaymentRegisterResult> UpdatePaymentRegisterAsync(UpdatePaymentRegisterRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a payment register's active status.
    /// </summary>
    /// <param name="request">The payment register status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePaymentRegisterStatusResult> UpdatePaymentRegisterStatusAsync(UpdatePaymentRegisterStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment register (soft delete).
    /// </summary>
    /// <param name="request">The delete payment register request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeletePaymentRegisterResult> DeletePaymentRegisterAsync(DeletePaymentRegisterRequest request, CancellationToken cancellationToken = default);
}
