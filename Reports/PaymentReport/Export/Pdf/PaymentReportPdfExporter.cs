using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.PaymentReport.Contracts;
using veteran_logistic.Reports.PaymentReport.DTOs;

namespace veteran_logistic.Reports.PaymentReport.Export.Pdf;

/// <summary>
/// Implementation of the payment report PDF exporter.
/// </summary>
public sealed class PaymentReportPdfExporter : IPaymentReportPdfExporter
{
    private readonly ILogger<PaymentReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PaymentReportPdfExporter(ILogger<PaymentReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<PaymentReportItem> items,
        PaymentReportTotals totals,
        PaymentReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for payment report to {FilePath}", filePath);

        var document = new PaymentReportDocument(items, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class PaymentReportDocument : IDocument
    {
        private readonly IReadOnlyList<PaymentReportItem> _items;
        private readonly PaymentReportTotals _totals;
        private readonly PaymentReportFilter _filter;

        public PaymentReportDocument(
            IReadOnlyList<PaymentReportItem> items,
            PaymentReportTotals totals,
            PaymentReportFilter filter)
        {
            _items = items;
            _totals = totals;
            _filter = filter;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Calibri));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
                });
        }

        private void ComposeHeader(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text("Veteran Logistics").Bold().FontSize(16);
                column.Item().Text("Payment Report").Bold().FontSize(14);
                column.Item().LineHorizontal(1);
                column.Item().Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}").FontSize(8);
                
                if (_filter.HasFilter)
                {
                    column.Item().Text("Filters Applied:").Bold().FontSize(9);
                    if (_filter.DateFrom.HasValue || _filter.DateTo.HasValue)
                        column.Item().Text($"Date: {_filter.DateFrom:dd-MM-yyyy} to {_filter.DateTo:dd-MM-yyyy}").FontSize(8);
                }
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(0.8f);  // Payment Date
                        columns.RelativeColumn(1f);    // Challan
                        columns.RelativeColumn(0.6f);  // TP
                        columns.RelativeColumn(1f);    // Vehicle
                        columns.RelativeColumn(1.2f);  // Customer
                        columns.RelativeColumn(1f);    // Material
                        columns.RelativeColumn(1f);    // Driver
                        columns.RelativeColumn(1f);    // Owner
                        columns.RelativeColumn(0.8f);  // Payment Type
                        columns.RelativeColumn(1f);    // Beneficiary
                        columns.RelativeColumn(1f);    // Bank
                        columns.RelativeColumn(0.7f);  // UTR
                        columns.RelativeColumn(0.7f);  // Driver Comm
                        columns.RelativeColumn(0.7f);  // Challan Amt
                        columns.RelativeColumn(0.6f);  // TDS
                        columns.RelativeColumn(0.6f);  // Surcharge
                        columns.RelativeColumn(0.6f);  // Admin
                        columns.RelativeColumn(0.7f);  // Net Payment
                        columns.RelativeColumn(0.5f);  // Status
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Payment Date").Bold();
                        header.Cell().Element(CellStyle).Text("Challan").Bold();
                        header.Cell().Element(CellStyle).Text("TP").Bold();
                        header.Cell().Element(CellStyle).Text("Vehicle").Bold();
                        header.Cell().Element(CellStyle).Text("Customer").Bold();
                        header.Cell().Element(CellStyle).Text("Material").Bold();
                        header.Cell().Element(CellStyle).Text("Driver").Bold();
                        header.Cell().Element(CellStyle).Text("Owner").Bold();
                        header.Cell().Element(CellStyle).Text("Payment Type").Bold();
                        header.Cell().Element(CellStyle).Text("Beneficiary").Bold();
                        header.Cell().Element(CellStyle).Text("Bank").Bold();
                        header.Cell().Element(CellStyle).Text("UTR").Bold();
                        header.Cell().Element(CellStyle).Text("Driver Comm").Bold();
                        header.Cell().Element(CellStyle).Text("Challan Amt").Bold();
                        header.Cell().Element(CellStyle).Text("TDS").Bold();
                        header.Cell().Element(CellStyle).Text("Surcharge").Bold();
                        header.Cell().Element(CellStyle).Text("Admin").Bold();
                        header.Cell().Element(CellStyle).Text("Net Payment").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                    });

                    foreach (var item in _items)
                    {
                        table.Cell().Element(CellStyle).Text(item.PaymentDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.TPNumber);
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber);
                        table.Cell().Element(CellStyle).Text(item.CustomerName);
                        table.Cell().Element(CellStyle).Text(item.MaterialName);
                        table.Cell().Element(CellStyle).Text(item.Driver);
                        table.Cell().Element(CellStyle).Text(item.VehicleOwner);
                        table.Cell().Element(CellStyle).Text(item.PaymentType);
                        table.Cell().Element(CellStyle).Text(item.Beneficiary);
                        table.Cell().Element(CellStyle).Text(item.BankName);
                        table.Cell().Element(CellStyle).Text(item.UTRNumber);
                        table.Cell().Element(CellStyle).Text(item.DriverCommission.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.ChallanAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.TDSAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.SurchargeAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.AdminCharge.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.NetPayment.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.PaymentStatus);
                    }
                });

                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Cell().Element(CellStyle).Text($"Total Records: {_totals.RecordCount}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Loading Weight: {_totals.TotalLoadingWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Unloading Weight: {_totals.TotalUnloadingWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Driver Comm: {_totals.TotalDriverCommission:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Challan Amt: {_totals.TotalChallanAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total TDS: {_totals.TotalTDSAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Surcharge: {_totals.TotalSurchargeAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Admin: {_totals.TotalAdminCharge:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Net Payment: {_totals.TotalNetPayment:F2}").Bold();
                });
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(2)
                .AlignCenter()
                .AlignMiddle();
        }
    }
}
