using veteran_logistic.Reports.PaymentReport.DTOs;

namespace veteran_logistic.Reports.PaymentReport.Contracts;

/// <summary>
/// Service for querying payment report data.
/// </summary>
public interface IPaymentReportQueryService
{
    /// <summary>
    /// Gets payment report data with applied filters, search, and sorting.
    /// </summary>
    /// <param name="filter">The filter criteria.</param>
    /// <param name="search">The search text.</param>
    /// <param name="sortBy">The sort field.</param>
    /// <param name="sortAscending">Whether to sort ascending.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A tuple containing the report items and calculated totals.</returns>
    Task<(IReadOnlyList<PaymentReportItem> Items, PaymentReportTotals Totals)> GetPaymentReportAsync(
        PaymentReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default);
}
