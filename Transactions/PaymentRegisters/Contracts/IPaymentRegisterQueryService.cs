using veteran_logistic.Transactions.PaymentRegisters.Models;

namespace veteran_logistic.Transactions.PaymentRegisters.Contracts;

/// <summary>
/// Service contract for querying payment register data.
/// </summary>
public interface IPaymentRegisterQueryService
{
    /// <summary>
    /// Gets all payment registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of payment register list items.</returns>
    Task<IReadOnlyList<PaymentRegisterListItem>> GetAllPaymentRegistersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches payment registers by challan number, TP number, vehicle number, material, beneficiary, or payment status.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of payment register list items matching the search criteria.</returns>
    Task<IReadOnlyList<PaymentRegisterListItem>> SearchPaymentRegistersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a payment register for editing by payment register ID.
    /// </summary>
    /// <param name="id">The payment register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment register model, or null if not found.</returns>
    Task<PaymentRegisterModel?> GetPaymentRegisterForEditAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a payment register by challan number.
    /// </summary>
    /// <param name="challanNumber">The challan number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment register model, or null if not found.</returns>
    Task<PaymentRegisterModel?> GetPaymentRegisterByChallanNumberAsync(string challanNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payment register data by challan number from Loading and Unloading registers.
    /// </summary>
    /// <param name="challanNumber">The challan number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The payment register model with auto-populated data, or null if not found.</returns>
    Task<PaymentRegisterModel?> GetPaymentRegisterDataByChallanNumberAsync(string challanNumber, CancellationToken cancellationToken = default);
}
