using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.LoadingReport.Contracts;
using veteran_logistic.Reports.LoadingReport.DTOs;

namespace veteran_logistic.Reports.LoadingReport.Export.Pdf;

/// <summary>
/// Implementation of the loading report PDF exporter.
/// </summary>
public sealed class LoadingReportPdfExporter : ILoadingReportPdfExporter
{
    private readonly ILogger<LoadingReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public LoadingReportPdfExporter(ILogger<LoadingReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<LoadingReportItem> items,
        LoadingReportTotals totals,
        LoadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for loading report to {FilePath}", filePath);

        var document = new LoadingReportDocument(items, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class LoadingReportDocument : IDocument
    {
        private readonly IReadOnlyList<LoadingReportItem> _items;
        private readonly LoadingReportTotals _totals;
        private readonly LoadingReportFilter _filter;

        public LoadingReportDocument(
            IReadOnlyList<LoadingReportItem> items,
            LoadingReportTotals totals,
            LoadingReportFilter filter)
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
                column.Item().Text("Loading Report").Bold().FontSize(14);
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
                        header.Cell().Element(CellStyle).Text("Rate").Bold();
                        header.Cell().Element(CellStyle).Text("Amount").Bold();
                        header.Cell().Element(CellStyle).Text("Active").Bold();
                    });

                    foreach (var item in _items)
                    {
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.LoadingDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber);
                        table.Cell().Element(CellStyle).Text(item.ConsignorName);
                        table.Cell().Element(CellStyle).Text(item.SourceName);
                        table.Cell().Element(CellStyle).Text(item.DestinationName);
                        table.Cell().Element(CellStyle).Text(item.LoadingWeight.ToString("F2"));
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
                    });

                    table.Cell().Element(CellStyle).Text($"Total Records: {_totals.RecordCount}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Weight: {_totals.TotalLoadingWeight:F2}").Bold();
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
