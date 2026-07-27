using veteran_logistic.Transactions.UnloadingRegisters.Models;

namespace veteran_logistic.Transactions.UnloadingRegisters.Contracts;

/// <summary>
/// Service contract for querying unloading register data.
/// </summary>
public interface IUnloadingRegisterQueryService
{
    /// <summary>
    /// Gets all unloading registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of unloading register list items.</returns>
    Task<IReadOnlyList<UnloadingRegisterListItem>> GetAllUnloadingRegistersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches unloading registers by challan number, TP number, vehicle number, consignor, consignee, driver, or material.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of unloading register list items matching the search criteria.</returns>
    Task<IReadOnlyList<UnloadingRegisterListItem>> SearchUnloadingRegistersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an unloading register for editing by unloading register ID.
    /// </summary>
    /// <param name="id">The unloading register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The unloading register model, or null if not found.</returns>
    Task<UnloadingRegisterModel?> GetUnloadingRegisterForEditAsync(int id, CancellationToken cancellationToken = default);
}
