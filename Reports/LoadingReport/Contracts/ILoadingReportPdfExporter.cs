using veteran_logistic.Reports.LoadingReport.DTOs;

namespace veteran_logistic.Reports.LoadingReport.Contracts;

/// <summary>
/// Service for exporting loading report to PDF.
/// </summary>
public interface ILoadingReportPdfExporter
{
    /// <summary>
    /// Exports the loading report to a PDF file.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The report totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The output file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the export operation.</returns>
    Task ExportToPdfAsync(
        IReadOnlyList<LoadingReportItem> items,
        LoadingReportTotals totals,
        LoadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
