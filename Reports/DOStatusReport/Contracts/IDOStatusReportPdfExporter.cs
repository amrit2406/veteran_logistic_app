using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Contracts;

/// <summary>
/// Service for exporting DO status report to PDF.
/// </summary>
public interface IDOStatusReportPdfExporter
{
    /// <summary>
    /// Exports the DO status report to a PDF file.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="summaryCards">The summary cards.</param>
    /// <param name="totals">The report totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The output file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the export operation.</returns>
    Task ExportToPdfAsync(
        IReadOnlyList<DOStatusReportItem> items,
        DOStatusReportSummaryCards summaryCards,
        DOStatusReportTotals totals,
        DOStatusReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
