using veteran_logistic.Reports.LoadingReport.DTOs;

namespace veteran_logistic.Reports.LoadingReport.Contracts;

/// <summary>
/// Service for querying loading report data.
/// </summary>
public interface ILoadingReportQueryService
{
    /// <summary>
    /// Gets loading report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items and calculated totals.</returns>
    Task<(IReadOnlyList<LoadingReportItem> Items, LoadingReportTotals Totals)> GetLoadingReportAsync(
        LoadingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);
}
