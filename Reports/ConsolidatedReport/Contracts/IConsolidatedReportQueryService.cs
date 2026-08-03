using veteran_logistic.Reports.ConsolidatedReport.DTOs;

namespace veteran_logistic.Reports.ConsolidatedReport.Contracts;

/// <summary>
/// Service for querying consolidated report data that combines Loading, Unloading, Payment, and Billing stages.
/// </summary>
public interface IConsolidatedReportQueryService
{
    /// <summary>
    /// Gets consolidated report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items, totals, and summary cards.</returns>
    Task<(IReadOnlyList<ConsolidatedReportItem> Items, ConsolidatedReportTotals Totals, ConsolidatedReportSummaryCards SummaryCards)> GetConsolidatedReportAsync(
        ConsolidatedReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);
}
