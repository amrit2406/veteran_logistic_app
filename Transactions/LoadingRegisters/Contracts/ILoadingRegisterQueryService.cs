using veteran_logistic.Transactions.LoadingRegisters.Models;

namespace veteran_logistic.Transactions.LoadingRegisters.Contracts;

/// <summary>
/// Service contract for querying loading register data.
/// </summary>
public interface ILoadingRegisterQueryService
{
    /// <summary>
    /// Gets all loading registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of loading register list items.</returns>
    Task<IReadOnlyList<LoadingRegisterListItem>> GetAllLoadingRegistersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches loading registers by challan number, TP number, vehicle number, consignor, consignee, driver, or material.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of loading register list items matching the search criteria.</returns>
    Task<IReadOnlyList<LoadingRegisterListItem>> SearchLoadingRegistersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a loading register for editing by loading register ID.
    /// </summary>
    /// <param name="id">The loading register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The loading register model, or null if not found.</returns>
    Task<LoadingRegisterModel?> GetLoadingRegisterForEditAsync(int id, CancellationToken cancellationToken = default);
}
