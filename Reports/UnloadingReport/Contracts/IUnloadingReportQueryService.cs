using veteran_logistic.Reports.UnloadingReport.DTOs;

namespace veteran_logistic.Reports.UnloadingReport.Contracts;

/// <summary>
/// Service for querying unloading report data.
/// </summary>
public interface IUnloadingReportQueryService
{
    /// <summary>
    /// Gets unloading report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items and calculated totals.</returns>
    Task<(IReadOnlyList<UnloadingReportItem> Items, UnloadingReportTotals Totals)> GetUnloadingReportAsync(
        UnloadingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);
}
