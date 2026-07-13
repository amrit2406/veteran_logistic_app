using veteran_logistic.Masters.PaymentLocations.Models;

namespace veteran_logistic.Masters.PaymentLocations.Contracts;

/// <summary>
/// Service contract for payment location command operations.
/// </summary>
public interface IPaymentLocationCommandService
{
    /// <summary>
    /// Creates a new payment location.
    /// </summary>
    /// <param name="request">The payment location creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created payment location ID.</returns>
    Task<CreatePaymentLocationResult> CreatePaymentLocationAsync(CreatePaymentLocationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing payment location.
    /// </summary>
    /// <param name="request">The payment location update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePaymentLocationResult> UpdatePaymentLocationAsync(UpdatePaymentLocationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a payment location's active status.
    /// </summary>
    /// <param name="request">The payment location status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdatePaymentLocationStatusResult> UpdatePaymentLocationStatusAsync(UpdatePaymentLocationStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a payment location (soft delete).
    /// </summary>
    /// <param name="request">The delete payment location request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeletePaymentLocationResult> DeletePaymentLocationAsync(DeletePaymentLocationRequest request, CancellationToken cancellationToken = default);
}
