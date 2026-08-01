using veteran_logistic.Reports.TdsReport.DTOs;

namespace veteran_logistic.Reports.TdsReport.Contracts;

/// <summary>
/// Service for exporting TDS report to PDF.
/// </summary>
public interface ITdsReportPdfExporter
{
    /// <summary>
    /// Exports the TDS report to PDF.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The destination file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToPdfAsync(
        IReadOnlyList<TdsReportItem> items,
        TdsReportTotals totals,
        TdsReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
