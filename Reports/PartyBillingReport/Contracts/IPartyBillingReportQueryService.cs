using veteran_logistic.Reports.PartyBillingReport.DTOs;

namespace veteran_logistic.Reports.PartyBillingReport.Contracts;

/// <summary>
/// Service for querying party billing report data.
/// </summary>
public interface IPartyBillingReportQueryService
{
    /// <summary>
    /// Gets party billing report summary data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items and calculated totals.</returns>
    Task<(IReadOnlyList<PartyBillingReportItem> Items, PartyBillingReportTotals Totals)> GetPartyBillingReportAsync(
        PartyBillingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets party billing report detail data for a specific bill.
    /// </summary>
    /// <param name="partyBillRegisterId">The party bill register ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The detail items for the specified bill.</returns>
    Task<IReadOnlyList<PartyBillingReportDetailItem>> GetPartyBillingReportDetailsAsync(
        int partyBillRegisterId,
        CancellationToken cancellationToken = default);
}
