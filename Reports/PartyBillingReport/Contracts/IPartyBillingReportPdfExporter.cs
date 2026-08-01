using veteran_logistic.Reports.PartyBillingReport.DTOs;

namespace veteran_logistic.Reports.PartyBillingReport.Contracts;

/// <summary>
/// Service for exporting party billing report to PDF.
/// </summary>
public interface IPartyBillingReportPdfExporter
{
    /// <summary>
    /// Exports the party billing report to PDF.
    /// </summary>
    /// <param name="summaryItems">The summary items (bills).</param>
    /// <param name="detailItems">The detail items (bill details).</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="filter">The applied filter.</param>
    /// <param name="filePath">The output file path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToPdfAsync(
        IReadOnlyList<PartyBillingReportItem> summaryItems,
        IReadOnlyList<PartyBillingReportDetailItem> detailItems,
        PartyBillingReportTotals totals,
        PartyBillingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
