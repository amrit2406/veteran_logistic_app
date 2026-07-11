using veteran_logistic.Masters.Vendors.Models;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Service contract for querying vendor data.
/// </summary>
public interface IVendorQueryService
{
    /// <summary>
    /// Gets all vendors.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vendor list items.</returns>
    Task<IReadOnlyList<VendorListItem>> GetAllVendorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches vendors by vendor code, vendor name, city, or state.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of vendor list items matching the search criteria.</returns>
    Task<IReadOnlyList<VendorListItem>> SearchVendorsAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a vendor for editing by vendor ID.
    /// </summary>
    /// <param name="id">The vendor ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The vendor model, or null if not found.</returns>
    Task<VendorModel?> GetVendorForEditAsync(int id, CancellationToken cancellationToken = default);
}
