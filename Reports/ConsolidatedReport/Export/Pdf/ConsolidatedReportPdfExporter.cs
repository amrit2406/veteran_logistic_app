using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.ConsolidatedReport.Contracts;
using veteran_logistic.Reports.ConsolidatedReport.DTOs;

namespace veteran_logistic.Reports.ConsolidatedReport.Export.Pdf;

/// <summary>
/// Implementation of the consolidated report PDF exporter.
/// </summary>
public sealed class ConsolidatedReportPdfExporter : IConsolidatedReportPdfExporter
{
    private readonly ILogger<ConsolidatedReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsolidatedReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public ConsolidatedReportPdfExporter(ILogger<ConsolidatedReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<ConsolidatedReportItem> items,
        ConsolidatedReportTotals totals,
        ConsolidatedReportSummaryCards summaryCards,
        ConsolidatedReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for consolidated report to {FilePath}", filePath);

        var document = new ConsolidatedReportDocument(items, totals, summaryCards, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class ConsolidatedReportDocument : IDocument
    {
        private readonly IReadOnlyList<ConsolidatedReportItem> _items;
        private readonly ConsolidatedReportTotals _totals;
        private readonly ConsolidatedReportSummaryCards _summaryCards;
        private readonly ConsolidatedReportFilter _filter;

        public ConsolidatedReportDocument(
            IReadOnlyList<ConsolidatedReportItem> items,
            ConsolidatedReportTotals totals,
            ConsolidatedReportSummaryCards summaryCards,
            ConsolidatedReportFilter filter)
        {
            _items = items;
            _totals = totals;
            _summaryCards = summaryCards;
            _filter = filter;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(8).FontFamily(Fonts.Calibri));

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
                column.Item().Text("Veteran Logistics").Bold().FontSize(14);
                column.Item().Text("Consolidated Report").Bold().FontSize(12);
                column.Item().LineHorizontal(1);
                column.Item().Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}").FontSize(7);
                
                if (_filter.HasFilter)
                {
                    column.Item().Text("Filters Applied:").Bold().FontSize(8);
                    if (_filter.DateFrom.HasValue || _filter.DateTo.HasValue)
                        column.Item().Text($"Date: {_filter.DateFrom:dd-MM-yyyy} to {_filter.DateTo:dd-MM-yyyy}").FontSize(7);
                }
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                // Summary Cards
                column.Item().Element(ComposeSummaryCards);
                
                column.Item().PaddingTop(5).LineHorizontal(1);

                // Transaction Table
                column.Item().PaddingTop(5).Element(ComposeTransactionTable);

                column.Item().PaddingTop(5).LineHorizontal(1);

                // Totals
                column.Item().PaddingTop(5).Element(ComposeTotals);
            });
        }

        private void ComposeSummaryCards(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Total Transactions").Bold().FontSize(8);
                    header.Cell().Element(CellStyle).Text("Loading Only").Bold().FontSize(8);
                    header.Cell().Element(CellStyle).Text("Pending Unloading").Bold().FontSize(8);
                    header.Cell().Element(CellStyle).Text("Pending Payment").Bold().FontSize(8);
                    header.Cell().Element(CellStyle).Text("Completed").Bold().FontSize(8);
                });

                table.Cell().Element(CellStyle).Text(_summaryCards.TotalTransactions.ToString()).FontSize(8);
                table.Cell().Element(CellStyle).Text(_summaryCards.LoadingOnly.ToString()).FontSize(8);
                table.Cell().Element(CellStyle).Text(_summaryCards.PendingUnloading.ToString()).FontSize(8);
                table.Cell().Element(CellStyle).Text(_summaryCards.PendingPayment.ToString()).FontSize(8);
                table.Cell().Element(CellStyle).Text(_summaryCards.Completed.ToString()).FontSize(8);
            });
        }

        private void ComposeTransactionTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(0.8f);  // Challan
                    columns.RelativeColumn(0.7f);  // Date
                    columns.RelativeColumn(1f);    // Vehicle
                    columns.RelativeColumn(1f);    // Consignor
                    columns.RelativeColumn(0.8f);  // Source
                    columns.RelativeColumn(0.8f);  // Dest
                    columns.RelativeColumn(0.6f);  // Weight
                    columns.RelativeColumn(0.5f);  // Amount
                    columns.RelativeColumn(0.6f);  // Status
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Challan").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Date").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Vehicle").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Consignor").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Source").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Dest").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Weight").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Amount").Bold().FontSize(7);
                    header.Cell().Element(CellStyle).Text("Status").Bold().FontSize(7);
                });

                foreach (var item in _items)
                {
                    table.Cell().Element(CellStyle).Text(item.ChallanNumber).FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.LoadingDate.ToString("dd-MM-yyyy")).FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.VehicleNumber ?? "").FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.ConsignorName ?? "").FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.SourceName ?? "").FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.DestinationName ?? "").FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.LoadingWeight.ToString("F2")).FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.LoadingAmount.ToString("F2")).FontSize(7);
                    table.Cell().Element(CellStyle).Text(item.LifecycleStatus).FontSize(7);
                }
            });
        }

        private void ComposeTotals(IContainer container)
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

                table.Cell().Element(CellStyle).Text($"Total Records: {_totals.RecordCount}").Bold().FontSize(8);
                table.Cell().Element(CellStyle).Text($"Total Weight: {_totals.TotalLoadingWeight:F2}").Bold().FontSize(8);
                table.Cell().Element(CellStyle).Text($"Total Amount: {_totals.TotalLoadingAmount:F2}").Bold().FontSize(8);
                table.Cell().Element(CellStyle).Text($"Total Net Payment: {_totals.TotalNetPayment:F2}").Bold().FontSize(8);
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
