using veteran_logistic.Reports.UnloadingReport.DTOs;

namespace veteran_logistic.Reports.UnloadingReport.Contracts;

/// <summary>
/// Service for exporting unloading report to PDF.
/// </summary>
public interface IUnloadingReportPdfExporter
{
    /// <summary>
    /// Exports the unloading report to PDF.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The file path to save the PDF.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToPdfAsync(
        IReadOnlyList<UnloadingReportItem> items,
        UnloadingReportTotals totals,
        UnloadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
