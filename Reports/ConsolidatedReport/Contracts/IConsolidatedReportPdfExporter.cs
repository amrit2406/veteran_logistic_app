using veteran_logistic.Reports.ConsolidatedReport.DTOs;

namespace veteran_logistic.Reports.ConsolidatedReport.Contracts;

/// <summary>
/// Service for exporting consolidated report data to PDF format.
/// </summary>
public interface IConsolidatedReportPdfExporter
{
    /// <summary>
    /// Exports the consolidated report data to a PDF file.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="summaryCards">The summary cards (KPIs).</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The file path to save the PDF.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToPdfAsync(
        IReadOnlyList<ConsolidatedReportItem> items,
        ConsolidatedReportTotals totals,
        ConsolidatedReportSummaryCards summaryCards,
        ConsolidatedReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
