using veteran_logistic.Transactions.PartyBillRegister.Models;

namespace veteran_logistic.Transactions.PartyBillRegister.Contracts;

/// <summary>
/// Service contract for querying party bill register data.
/// </summary>
public interface IPartyBillRegisterQueryService
{
    /// <summary>
    /// Gets all party bill registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of party bill register list items.</returns>
    Task<IReadOnlyList<PartyBillRegisterListItem>> GetAllPartyBillRegistersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches party bill registers by bill number, party name, third party name, or permit number.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of party bill register list items matching the search criteria.</returns>
    Task<IReadOnlyList<PartyBillRegisterListItem>> SearchPartyBillRegistersAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a party bill register for editing by party bill register ID.
    /// </summary>
    /// <param name="id">The party bill register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The party bill register model, or null if not found.</returns>
    Task<PartyBillRegisterModel?> GetPartyBillRegisterForEditAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets eligible loading registers for bill generation based on filters.
    /// </summary>
    /// <param name="consignorId">The consignor ID filter (optional).</param>
    /// <param name="destinationId">The destination ID filter (optional).</param>
    /// <param name="fromDate">The from date filter (optional).</param>
    /// <param name="toDate">The to date filter (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of eligible loading register models.</returns>
    Task<IReadOnlyList<EligibleLoadingRegisterModel>> GetEligibleLoadingRegistersAsync(int? consignorId, int? destinationId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets party bill register details by party bill register ID.
    /// </summary>
    /// <param name="partyBillRegisterId">The party bill register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of party bill register detail models.</returns>
    Task<IReadOnlyList<PartyBillRegisterDetailModel>> GetPartyBillRegisterDetailsAsync(int partyBillRegisterId, CancellationToken cancellationToken = default);
}
