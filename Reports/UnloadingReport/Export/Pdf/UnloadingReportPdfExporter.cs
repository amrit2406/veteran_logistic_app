using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.UnloadingReport.Contracts;
using veteran_logistic.Reports.UnloadingReport.DTOs;

namespace veteran_logistic.Reports.UnloadingReport.Export.Pdf;

/// <summary>
/// Implementation of the unloading report PDF exporter.
/// </summary>
public sealed class UnloadingReportPdfExporter : IUnloadingReportPdfExporter
{
    private readonly ILogger<UnloadingReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnloadingReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public UnloadingReportPdfExporter(ILogger<UnloadingReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<UnloadingReportItem> items,
        UnloadingReportTotals totals,
        UnloadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for unloading report to {FilePath}", filePath);

        var document = new UnloadingReportDocument(items, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class UnloadingReportDocument : IDocument
    {
        private readonly IReadOnlyList<UnloadingReportItem> _items;
        private readonly UnloadingReportTotals _totals;
        private readonly UnloadingReportFilter _filter;

        public UnloadingReportDocument(
            IReadOnlyList<UnloadingReportItem> items,
            UnloadingReportTotals totals,
            UnloadingReportFilter filter)
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
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Calibri));

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
                column.Item().Text("Unloading Report").Bold().FontSize(14);
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
                        columns.RelativeColumn(1f);  // Challan
                        columns.RelativeColumn(1f);  // Date
                        columns.RelativeColumn(1.5f);  // Vehicle
                        columns.RelativeColumn(1.5f);  // Consignor
                        columns.RelativeColumn(1f);  // Source
                        columns.RelativeColumn(1f);  // Dest
                        columns.RelativeColumn(0.8f);  // Weight
                        columns.RelativeColumn(0.8f);  // Shortage
                        columns.RelativeColumn(0.8f);  // Rate
                        columns.RelativeColumn(0.8f);  // Amount
                        columns.RelativeColumn(0.5f);  // Active
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Challan").Bold();
                        header.Cell().Element(CellStyle).Text("Date").Bold();
                        header.Cell().Element(CellStyle).Text("Vehicle").Bold();
                        header.Cell().Element(CellStyle).Text("Consignor").Bold();
                        header.Cell().Element(CellStyle).Text("Source").Bold();
                        header.Cell().Element(CellStyle).Text("Dest").Bold();
                        header.Cell().Element(CellStyle).Text("Weight").Bold();
                        header.Cell().Element(CellStyle).Text("Shortage").Bold();
                        header.Cell().Element(CellStyle).Text("Rate").Bold();
                        header.Cell().Element(CellStyle).Text("Amount").Bold();
                        header.Cell().Element(CellStyle).Text("Active").Bold();
                    });

                    foreach (var item in _items)
                    {
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.UnloadingDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber);
                        table.Cell().Element(CellStyle).Text(item.ConsignorName);
                        table.Cell().Element(CellStyle).Text(item.SourceName);
                        table.Cell().Element(CellStyle).Text(item.DestinationName);
                        table.Cell().Element(CellStyle).Text(item.UnloadingWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.ShortageWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.Rate.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.GrossAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.IsActive ? "Yes" : "No");
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
                    });

                    table.Cell().Element(CellStyle).Text($"Total Records: {_totals.RecordCount}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Weight: {_totals.TotalUnloadingWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Shortage: {_totals.TotalShortageWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Amount: {_totals.TotalGrossAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Cash Adv: {_totals.TotalCashAdvance:F2}").Bold();
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
