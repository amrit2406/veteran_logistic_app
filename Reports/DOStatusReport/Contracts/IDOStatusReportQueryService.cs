using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Contracts;

/// <summary>
/// Service for querying DO status report data.
/// </summary>
public interface IDOStatusReportQueryService
{
    /// <summary>
    /// Gets DO status report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items, summary cards, and calculated totals.</returns>
    Task<(IReadOnlyList<DOStatusReportItem> Items, DOStatusReportSummaryCards SummaryCards, DOStatusReportTotals Totals)> GetDOStatusReportAsync(
        DOStatusReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);
}
