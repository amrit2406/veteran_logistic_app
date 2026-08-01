using veteran_logistic.Reports.TdsReport.DTOs;

namespace veteran_logistic.Reports.TdsReport.Contracts;

/// <summary>
/// Service for querying TDS report data.
/// </summary>
public interface ITdsReportQueryService
{
    /// <summary>
    /// Gets TDS report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items and calculated totals.</returns>
    Task<(IReadOnlyList<TdsReportItem> Items, TdsReportTotals Totals)> GetTdsReportAsync(
        TdsReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets TDS report grouped data for summary analysis.
    /// </summary>
    /// <param name="groupBy">The field to group by.</param>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of grouped summary items.</returns>
    Task<IReadOnlyList<TdsReportGroupSummary>> GetGroupedSummaryAsync(
        string groupBy,
        TdsReportFilter filter,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a grouped summary for TDS report analysis.
/// </summary>
public sealed class TdsReportGroupSummary
{
    /// <summary>
    /// Gets or sets the group key (e.g., customer name, PAN, bank name).
    /// </summary>
    public string GroupKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of transactions in the group.
    /// </summary>
    public int TransactionCount { get; set; }

    /// <summary>
    /// Gets or sets the total challan amount for the group.
    /// </summary>
    public decimal TotalChallanAmount { get; set; }

    /// <summary>
    /// Gets or sets the total TDS amount for the group.
    /// </summary>
    public decimal TotalTDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the average TDS amount for the group.
    /// </summary>
    public decimal AverageTDS { get; set; }
}
