using veteran_logistic.Reports.UnloadingReport.DTOs;

namespace veteran_logistic.Reports.UnloadingReport.Contracts;

/// <summary>
/// Service for exporting unloading report to Excel.
/// </summary>
public interface IUnloadingReportExcelExporter
{
    /// <summary>
    /// Exports the unloading report to Excel.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The file path to save the Excel file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToExcelAsync(
        IReadOnlyList<UnloadingReportItem> items,
        UnloadingReportTotals totals,
        UnloadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
