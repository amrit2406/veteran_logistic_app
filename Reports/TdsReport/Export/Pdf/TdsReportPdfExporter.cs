using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.TdsReport.Contracts;
using veteran_logistic.Reports.TdsReport.DTOs;

namespace veteran_logistic.Reports.TdsReport.Export.Pdf;

/// <summary>
/// Implementation of the TDS report PDF exporter.
/// </summary>
public sealed class TdsReportPdfExporter : ITdsReportPdfExporter
{
    private readonly ILogger<TdsReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TdsReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public TdsReportPdfExporter(ILogger<TdsReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<TdsReportItem> items,
        TdsReportTotals totals,
        TdsReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for TDS report to {FilePath}", filePath);

        var document = new TdsReportDocument(items, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class TdsReportDocument : IDocument
    {
        private readonly IReadOnlyList<TdsReportItem> _items;
        private readonly TdsReportTotals _totals;
        private readonly TdsReportFilter _filter;

        public TdsReportDocument(
            IReadOnlyList<TdsReportItem> items,
            TdsReportTotals totals,
            TdsReportFilter filter)
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
                column.Item().Text("TDS Report").Bold().FontSize(14);
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
                // Tax Summary Section
                column.Item().PaddingBottom(10).Element(ComposeTaxSummary);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(0.8f);  // Payment Date
                        columns.RelativeColumn(1f);    // Challan
                        columns.RelativeColumn(1.2f);  // Customer
                        columns.RelativeColumn(0.8f);  // Vehicle
                        columns.RelativeColumn(1f);    // Driver
                        columns.RelativeColumn(1f);    // Beneficiary
                        columns.RelativeColumn(0.8f);  // PAN
                        columns.RelativeColumn(1f);    // Bank
                        columns.RelativeColumn(0.6f);  // TDS %
                        columns.RelativeColumn(0.7f);  // TDS Amt
                        columns.RelativeColumn(0.6f);  // Surcharge
                        columns.RelativeColumn(0.6f);  // Admin
                        columns.RelativeColumn(0.7f);  // Net Payment
                        columns.RelativeColumn(0.5f);  // Status
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Payment Date").Bold();
                        header.Cell().Element(CellStyle).Text("Challan").Bold();
                        header.Cell().Element(CellStyle).Text("Customer").Bold();
                        header.Cell().Element(CellStyle).Text("Vehicle").Bold();
                        header.Cell().Element(CellStyle).Text("Driver").Bold();
                        header.Cell().Element(CellStyle).Text("Beneficiary").Bold();
                        header.Cell().Element(CellStyle).Text("PAN").Bold();
                        header.Cell().Element(CellStyle).Text("Bank").Bold();
                        header.Cell().Element(CellStyle).Text("TDS %").Bold();
                        header.Cell().Element(CellStyle).Text("TDS Amt").Bold();
                        header.Cell().Element(CellStyle).Text("Surcharge").Bold();
                        header.Cell().Element(CellStyle).Text("Admin").Bold();
                        header.Cell().Element(CellStyle).Text("Net Payment").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                    });

                    foreach (var item in _items)
                    {
                        table.Cell().Element(CellStyle).Text(item.PaymentDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.Customer);
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber);
                        table.Cell().Element(CellStyle).Text(item.Driver);
                        table.Cell().Element(CellStyle).Text(item.Beneficiary);
                        table.Cell().Element(CellStyle).Text(item.PAN);
                        table.Cell().Element(CellStyle).Text(item.BankName);
                        table.Cell().Element(CellStyle).Text($"{item.TDSPercentage:F2}%");
                        table.Cell().Element(CellStyle).Text(item.TDSAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.Surcharge.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.AdminCharge.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.NetPayment.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.PaymentStatus);
                    }
                });

                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(10).Element(ComposeGrandTotals);
            });
        }

        private void ComposeTaxSummary(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                table.Cell().Element(SummaryCellStyle).Text("Tax Summary").Bold().FontSize(10);
                table.Cell().Element(SummaryCellStyle).Text("").FontSize(10);
                table.Cell().Element(SummaryCellStyle).Text("").FontSize(10);
                table.Cell().Element(SummaryCellStyle).Text("").FontSize(10);

                table.Cell().Element(SummaryCellStyle).Text($"Total Challan: {_totals.TotalChallanAmount:F2}").FontSize(9);
                table.Cell().Element(SummaryCellStyle).Text($"Total TDS: {_totals.TotalTDSAmount:F2}").FontSize(9);
                table.Cell().Element(SummaryCellStyle).Text($"Avg TDS: {_totals.AverageTDSAmount:F2}").FontSize(9);
                table.Cell().Element(SummaryCellStyle).Text($"Records: {_totals.RecordCount}").FontSize(9);
            });
        }

        private void ComposeGrandTotals(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                table.Cell().Element(TotalsCellStyle).Text($"Total Challan: {_totals.TotalChallanAmount:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"Total TDS: {_totals.TotalTDSAmount:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"Total Surcharge: {_totals.TotalSurcharge:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"Total Admin: {_totals.TotalAdminCharge:F2}").Bold();

                table.Cell().Element(TotalsCellStyle).Text($"Total Net Payment: {_totals.TotalNetPayment:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"Highest TDS: {_totals.HighestTDSAmount:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"Lowest TDS: {_totals.LowestTDSAmount:F2}").Bold();
                table.Cell().Element(TotalsCellStyle).Text($"").Bold();
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

        private static IContainer SummaryCellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten3)
                .Padding(4)
                .AlignCenter()
                .AlignMiddle()
                .Background(Colors.Grey.Lighten5);
        }

        private static IContainer TotalsCellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(3)
                .AlignCenter()
                .AlignMiddle()
                .Background(Colors.Grey.Lighten4);
        }
    }
}
