using veteran_logistic.Reports.PaymentReport.DTOs;

namespace veteran_logistic.Reports.PaymentReport.Contracts;

/// <summary>
/// Service for exporting payment report to PDF.
/// </summary>
public interface IPaymentReportPdfExporter
{
    /// <summary>
    /// Exports the payment report to PDF.
    /// </summary>
    /// <param name="items">The report items to export.</param>
    /// <param name="totals">The calculated totals.</param>
    /// <param name="filter">The applied filter criteria.</param>
    /// <param name="filePath">The file path to save the PDF.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ExportToPdfAsync(
        IReadOnlyList<PaymentReportItem> items,
        PaymentReportTotals totals,
        PaymentReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default);
}
