using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.PartyBillingReport.Contracts;
using veteran_logistic.Reports.PartyBillingReport.DTOs;

namespace veteran_logistic.Reports.PartyBillingReport.Export.Pdf;

/// <summary>
/// Implementation of the party billing report PDF exporter.
/// </summary>
public sealed class PartyBillingReportPdfExporter : IPartyBillingReportPdfExporter
{
    private readonly ILogger<PartyBillingReportPdfExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillingReportPdfExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PartyBillingReportPdfExporter(ILogger<PartyBillingReportPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToPdfAsync(
        IReadOnlyList<PartyBillingReportItem> summaryItems,
        IReadOnlyList<PartyBillingReportDetailItem> detailItems,
        PartyBillingReportTotals totals,
        PartyBillingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for party billing report to {FilePath}", filePath);

        var document = new PartyBillingReportDocument(summaryItems, detailItems, totals, filter);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {SummaryCount} bills and {DetailCount} details", 
            summaryItems.Count, detailItems.Count);
        await Task.CompletedTask;
    }

    private class PartyBillingReportDocument : IDocument
    {
        private readonly IReadOnlyList<PartyBillingReportItem> _summaryItems;
        private readonly IReadOnlyList<PartyBillingReportDetailItem> _detailItems;
        private readonly PartyBillingReportTotals _totals;
        private readonly PartyBillingReportFilter _filter;

        public PartyBillingReportDocument(
            IReadOnlyList<PartyBillingReportItem> summaryItems,
            IReadOnlyList<PartyBillingReportDetailItem> detailItems,
            PartyBillingReportTotals totals,
            PartyBillingReportFilter filter)
        {
            _summaryItems = summaryItems;
            _detailItems = detailItems;
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
                column.Item().Text("Party Wise Billing Report").Bold().FontSize(14);
                column.Item().LineHorizontal(1);
                column.Item().Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}").FontSize(8);
                
                if (_filter.HasFilter)
                {
                    column.Item().Text("Filters Applied:").Bold().FontSize(9);
                    if (_filter.BillDateFrom.HasValue || _filter.BillDateTo.HasValue)
                        column.Item().Text($"Bill Date: {_filter.BillDateFrom:dd-MM-yyyy} to {_filter.BillDateTo:dd-MM-yyyy}").FontSize(8);
                    if (_filter.LoadingDateFrom.HasValue || _filter.LoadingDateTo.HasValue)
                        column.Item().Text($"Loading Date: {_filter.LoadingDateFrom:dd-MM-yyyy} to {_filter.LoadingDateTo:dd-MM-yyyy}").FontSize(8);
                }
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                // Summary Section
                column.Item().Text("Bill Summary").Bold().FontSize(12);
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1f);    // Bill No
                        columns.RelativeColumn(0.8f);  // Bill Date
                        columns.RelativeColumn(1.2f);  // Customer
                        columns.RelativeColumn(1f);    // Third Party
                        columns.RelativeColumn(0.8f);  // Permit No
                        columns.RelativeColumn(0.7f);  // From Date
                        columns.RelativeColumn(0.7f);  // To Date
                        columns.RelativeColumn(0.6f);  // Challans
                        columns.RelativeColumn(0.8f);  // Weight
                        columns.RelativeColumn(0.8f);  // Amount
                        columns.RelativeColumn(0.5f);  // Status
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Bill No").Bold();
                        header.Cell().Element(CellStyle).Text("Bill Date").Bold();
                        header.Cell().Element(CellStyle).Text("Customer").Bold();
                        header.Cell().Element(CellStyle).Text("Third Party").Bold();
                        header.Cell().Element(CellStyle).Text("Permit No").Bold();
                        header.Cell().Element(CellStyle).Text("From Date").Bold();
                        header.Cell().Element(CellStyle).Text("To Date").Bold();
                        header.Cell().Element(CellStyle).Text("Challans").Bold();
                        header.Cell().Element(CellStyle).Text("Weight").Bold();
                        header.Cell().Element(CellStyle).Text("Amount").Bold();
                        header.Cell().Element(CellStyle).Text("Status").Bold();
                    });

                    foreach (var item in _summaryItems)
                    {
                        table.Cell().Element(CellStyle).Text(item.BillNumber);
                        table.Cell().Element(CellStyle).Text(item.BillDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.Customer);
                        table.Cell().Element(CellStyle).Text(item.ThirdParty);
                        table.Cell().Element(CellStyle).Text(item.PermitNumber ?? "");
                        table.Cell().Element(CellStyle).Text(item.FromDate?.ToString("dd-MM-yyyy") ?? "");
                        table.Cell().Element(CellStyle).Text(item.ToDate?.ToString("dd-MM-yyyy") ?? "");
                        table.Cell().Element(CellStyle).Text(item.NumberOfChallans.ToString());
                        table.Cell().Element(CellStyle).Text(item.TotalLoadingWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.TotalBillAmount.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.Status);
                    }
                });

                // Details Section
                column.Item().PaddingTop(15).Text("Bill Details").Bold().FontSize(12);
                column.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1f);    // Challan No
                        columns.RelativeColumn(0.7f);  // Loading Date
                        columns.RelativeColumn(0.8f);  // Vehicle
                        columns.RelativeColumn(1f);    // Material
                        columns.RelativeColumn(1f);    // Consignor
                        columns.RelativeColumn(1f);    // Consignee
                        columns.RelativeColumn(0.8f);  // Source
                        columns.RelativeColumn(0.8f);  // Destination
                        columns.RelativeColumn(0.7f);  // Weight
                        columns.RelativeColumn(0.6f);  // Rate
                        columns.RelativeColumn(0.7f);  // Gross Amount
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Challan No").Bold();
                        header.Cell().Element(CellStyle).Text("Loading Date").Bold();
                        header.Cell().Element(CellStyle).Text("Vehicle").Bold();
                        header.Cell().Element(CellStyle).Text("Material").Bold();
                        header.Cell().Element(CellStyle).Text("Consignor").Bold();
                        header.Cell().Element(CellStyle).Text("Consignee").Bold();
                        header.Cell().Element(CellStyle).Text("Source").Bold();
                        header.Cell().Element(CellStyle).Text("Destination").Bold();
                        header.Cell().Element(CellStyle).Text("Weight").Bold();
                        header.Cell().Element(CellStyle).Text("Rate").Bold();
                        header.Cell().Element(CellStyle).Text("Gross Amount").Bold();
                    });

                    foreach (var item in _detailItems)
                    {
                        table.Cell().Element(CellStyle).Text(item.ChallanNumber);
                        table.Cell().Element(CellStyle).Text(item.LoadingDate.ToString("dd-MM-yyyy"));
                        table.Cell().Element(CellStyle).Text(item.VehicleNumber ?? "");
                        table.Cell().Element(CellStyle).Text(item.Material ?? "");
                        table.Cell().Element(CellStyle).Text(item.Consignor ?? "");
                        table.Cell().Element(CellStyle).Text(item.Consignee ?? "");
                        table.Cell().Element(CellStyle).Text(item.Source ?? "");
                        table.Cell().Element(CellStyle).Text(item.Destination ?? "");
                        table.Cell().Element(CellStyle).Text(item.LoadingWeight.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.Rate.ToString("F2"));
                        table.Cell().Element(CellStyle).Text(item.GrossAmount.ToString("F2"));
                    }
                });

                // Totals Section
                column.Item().LineHorizontal(1);
                column.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2f);
                        columns.RelativeColumn(1f);
                    });

                    table.Cell().Element(CellStyle).Text("Record Count:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.RecordCount.ToString()).AlignRight();

                    table.Cell().Element(CellStyle).Text("Total Bills:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.TotalBills.ToString()).AlignRight();

                    table.Cell().Element(CellStyle).Text("Total Challans:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.TotalChallans.ToString()).AlignRight();

                    table.Cell().Element(CellStyle).Text("Total Loading Weight:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.TotalLoadingWeight.ToString("F2")).AlignRight();

                    table.Cell().Element(CellStyle).Text("Total Gross Amount:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.TotalGrossAmount.ToString("F2")).AlignRight();

                    table.Cell().Element(CellStyle).Text("Average Bill Amount:").Bold();
                    table.Cell().Element(CellStyle).Text(_totals.AverageBillAmount.ToString("F2")).AlignRight();
                });
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(2)
                .AlignCenter()
                .AlignMiddle();
        }
    }
}
