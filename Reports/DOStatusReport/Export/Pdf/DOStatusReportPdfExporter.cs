using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.DOStatusReport.Contracts;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Export.Pdf;

/// <summary>
/// Implementation of the DO status report PDF exporter.
/// </summary>
public sealed class DOStatusReportPdfExporter : IDOStatusReportPdfExporter
{
    private readonly ILogger<DOStatusReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DOStatusReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public DOStatusReportPdfExporter(ILogger<DOStatusReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<DOStatusReportItem> items,
        DOStatusReportSummaryCards summaryCards,
        DOStatusReportTotals totals,
        DOStatusReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for DO status report to {FilePath}", filePath);

        var document = new DOStatusReportDocument(items, summaryCards, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private class DOStatusReportDocument : IDocument
    {
        private readonly IReadOnlyList<DOStatusReportItem> _items;
        private readonly DOStatusReportSummaryCards _summaryCards;
        private readonly DOStatusReportTotals _totals;
        private readonly DOStatusReportFilter _filter;

        public DOStatusReportDocument(
            IReadOnlyList<DOStatusReportItem> items,
            DOStatusReportSummaryCards summaryCards,
            DOStatusReportTotals totals,
            DOStatusReportFilter filter)
        {
            _items = items;
            _summaryCards = summaryCards;
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
                column.Item().Text("DO Status Report").Bold().FontSize(14);
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
                // Summary Cards
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Cell().Element(CellStyle).Text($"Total DO: {_summaryCards.TotalDO}").Bold();
                    table.Cell().Element(CellStyle).Text($"Today's Loading: {_summaryCards.TodayLoading}").Bold();
                    table.Cell().Element(CellStyle).Text($"Today's Completed: {_summaryCards.TodayCompleted}").Bold();
                    table.Cell().Element(CellStyle).Text($"Running DO: {_summaryCards.RunningDO}").Bold();
                    table.Cell().Element(CellStyle).Text($"Completed DO: {_summaryCards.CompletedDO}").Bold();
                    table.Cell().Element(CellStyle).Text($"Payment Pending: {_summaryCards.PaymentPending}").Bold();
                    table.Cell().Element(CellStyle).Text($"Bill Pending: {_summaryCards.BillPending}").Bold();
                    table.Cell().Element(CellStyle).Text($"Delayed DO: {_summaryCards.DelayedDO}").Bold();
                    table.Cell().Element(CellStyle).Text($"Exception DO: {_summaryCards.ExceptionDO}").Bold();
                    table.Cell().Element(CellStyle).Text($"Completion %: {_summaryCards.CompletionPercentage:F1}%").Bold();
                    table.Cell().Element(CellStyle).Text($"Pending %: {_summaryCards.PendingPercentage:F1}%").Bold();
                });

                column.Item().PaddingTop(10);

                // Main Data Table
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(0.7f);  // Challan
                        columns.RelativeColumn(0.5f);  // TP
                        columns.RelativeColumn(0.7f);  // Date
                        columns.RelativeColumn(1.0f);  // Vehicle
                        columns.RelativeColumn(1.0f);  // Consignor
                        columns.RelativeColumn(1.0f);  // Consignee
                        columns.RelativeColumn(0.6f);  // Load Weight
                        columns.RelativeColumn(0.6f);  // Unload Weight
                        columns.RelativeColumn(0.5f);  // Shortage
                        columns.RelativeColumn(0.6f);  // Gross Amount
                        columns.RelativeColumn(0.6f);  // Challan Money
                        columns.RelativeColumn(0.6f);  // Pending Amount
                        columns.RelativeColumn(0.5f);  // Age
                        columns.RelativeColumn(0.4f);  // Delayed
                        columns.RelativeColumn(0.6f);  // DO Status
                        columns.RelativeColumn(0.6f);  // Pay Status
                        columns.RelativeColumn(0.6f);  // Bill Status
                        columns.RelativeColumn(0.6f);  // Exception
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Challan").Bold();
                        header.Cell().Element(CellStyle).Text("TP").Bold();
                        header.Cell().Element(CellStyle).Text("Date").Bold();
                        header.Cell().Element(CellStyle).Text("Vehicle").Bold();
                        header.Cell().Element(CellStyle).Text("Consignor").Bold();
                        header.Cell().Element(CellStyle).Text("Consignee").Bold();
                        header.Cell().Element(CellStyle).Text("Load Wt").Bold();
                        header.Cell().Element(CellStyle).Text("Unload Wt").Bold();
                        header.Cell().Element(CellStyle).Text("Shortage").Bold();
                        header.Cell().Element(CellStyle).Text("Gross Amt").Bold();
                        header.Cell().Element(CellStyle).Text("Challan $").Bold();
                        header.Cell().Element(CellStyle).Text("Pending $").Bold();
                        header.Cell().Element(CellStyle).Text("Age").Bold();
                        header.Cell().Element(CellStyle).Text("Delayed").Bold();
                        header.Cell().Element(CellStyle).Text("DO Status").Bold();
                        header.Cell().Element(CellStyle).Text("Pay Status").Bold();
                        header.Cell().Element(CellStyle).Text("Bill Status").Bold();
                        header.Cell().Element(CellStyle).Text("Exception").Bold();
                    });

                    foreach (var item in _items)
                    {
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.TPNumber);
                        table.Cell().Element(CellStyle).Text(item.LoadingDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber);
                        table.Cell().Element(CellStyle).Text(item.ConsignorName);
                        table.Cell().Element(CellStyle).Text(item.ConsigneeName);
                        table.Cell().Element(CellStyle).Text(item.LoadingWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.UnloadingWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.ShortageWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.GrossAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.ChallanMoney.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.PendingAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.AgeInDays.ToString());
                        table.Cell().Element(CellStyle).Text(item.IsDelayed ? "Yes" : "No");
                        table.Cell().Element(CellStyle).Text(item.DOStatus.ToString());
                        table.Cell().Element(CellStyle).Text(item.PaymentStatus.ToString());
                        table.Cell().Element(CellStyle).Text(item.BillingStatus.ToString());
                        table.Cell().Element(CellStyle).Text(item.ExceptionType.ToString());
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
                    });

                    table.Cell().Element(CellStyle).Text($"Total Records: {_totals.TotalRecords}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Load Wt: {_totals.TotalLoadingWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Unload Wt: {_totals.TotalUnloadingWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Shortage: {_totals.TotalShortageWeight:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Gross Amt: {_totals.TotalGrossAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Challan $: {_totals.TotalChallanMoney:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Total Pending: {_totals.TotalPendingAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Completed Gross: {_totals.CompletedGrossAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Pending Gross: {_totals.PendingGrossAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Today's Gross: {_totals.TodayGrossAmount:F2}").Bold();
                    table.Cell().Element(CellStyle).Text($"Today's Load Wt: {_totals.TodayLoadingWeight:F2}").Bold();
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
