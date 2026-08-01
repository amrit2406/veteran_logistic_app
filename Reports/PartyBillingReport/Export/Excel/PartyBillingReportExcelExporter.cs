using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.PartyBillingReport.Contracts;
using veteran_logistic.Reports.PartyBillingReport.DTOs;

namespace veteran_logistic.Reports.PartyBillingReport.Export.Excel;

/// <summary>
/// Implementation of the party billing report Excel exporter.
/// </summary>
public sealed class PartyBillingReportExcelExporter : IPartyBillingReportExcelExporter
{
    private readonly ILogger<PartyBillingReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillingReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PartyBillingReportExcelExporter(ILogger<PartyBillingReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<PartyBillingReportItem> summaryItems,
        IReadOnlyList<PartyBillingReportDetailItem> detailItems,
        PartyBillingReportTotals totals,
        PartyBillingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for party billing report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();

        // Worksheet 1: Bill Summary
        var summaryWorksheet = workbook.Worksheets.Add("Bill Summary");
        ComposeSummaryWorksheet(summaryWorksheet, summaryItems, totals, filter);

        // Worksheet 2: Bill Details
        var detailsWorksheet = workbook.Worksheets.Add("Bill Details");
        ComposeDetailsWorksheet(detailsWorksheet, detailItems);

        workbook.SaveAs(filePath);

        _logger.LogInformation("Excel export completed successfully for {SummaryCount} bills and {DetailCount} details", 
            summaryItems.Count, detailItems.Count);
        await Task.CompletedTask;
    }

    private void ComposeSummaryWorksheet(IXLWorksheet worksheet, IReadOnlyList<PartyBillingReportItem> items, 
        PartyBillingReportTotals totals, PartyBillingReportFilter filter)
    {
        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "Party Wise Billing Report - Bill Summary";
        worksheet.Cell("A2").Style.Font.Bold = true;
        worksheet.Cell("A2").Style.Font.FontSize = 12;

        worksheet.Cell("A3").Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";
        worksheet.Cell("A3").Style.Font.FontSize = 9;

        // Filters
        if (filter.HasFilter)
        {
            int filterRow = 5;
            worksheet.Cell($"A{filterRow}").Value = "Filters Applied:";
            worksheet.Cell($"A{filterRow}").Style.Font.Bold = true;
            
            if (filter.BillDateFrom.HasValue || filter.BillDateTo.HasValue)
            {
                filterRow++;
                worksheet.Cell($"A{filterRow}").Value = $"Bill Date: {filter.BillDateFrom:dd-MM-yyyy} to {filter.BillDateTo:dd-MM-yyyy}";
            }
            if (filter.LoadingDateFrom.HasValue || filter.LoadingDateTo.HasValue)
            {
                filterRow++;
                worksheet.Cell($"A{filterRow}").Value = $"Loading Date: {filter.LoadingDateFrom:dd-MM-yyyy} to {filter.LoadingDateTo:dd-MM-yyyy}";
            }
        }

        // Table Headers
        int headerRow = filter.HasFilter ? 8 : 5;
        worksheet.Cell($"A{headerRow}").Value = "Bill No.";
        worksheet.Cell($"B{headerRow}").Value = "Bill Date";
        worksheet.Cell($"C{headerRow}").Value = "Customer";
        worksheet.Cell($"D{headerRow}").Value = "Third Party";
        worksheet.Cell($"E{headerRow}").Value = "Permit No.";
        worksheet.Cell($"F{headerRow}").Value = "From Date";
        worksheet.Cell($"G{headerRow}").Value = "To Date";
        worksheet.Cell($"H{headerRow}").Value = "No. of Challans";
        worksheet.Cell($"I{headerRow}").Value = "Total Weight";
        worksheet.Cell($"J{headerRow}").Value = "Total Amount";
        worksheet.Cell($"K{headerRow}").Value = "Status";

        var headerRange = worksheet.Range($"A{headerRow}:K{headerRow}");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Data
        int dataRow = headerRow + 1;
        foreach (var item in items)
        {
            worksheet.Cell($"A{dataRow}").Value = item.BillNumber;
            worksheet.Cell($"B{dataRow}").Value = item.BillDate;
            worksheet.Cell($"B{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"C{dataRow}").Value = item.Customer;
            worksheet.Cell($"D{dataRow}").Value = item.ThirdParty;
            worksheet.Cell($"E{dataRow}").Value = item.PermitNumber;
            worksheet.Cell($"F{dataRow}").Value = item.FromDate;
            worksheet.Cell($"F{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"G{dataRow}").Value = item.ToDate;
            worksheet.Cell($"G{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"H{dataRow}").Value = item.NumberOfChallans;
            worksheet.Cell($"I{dataRow}").Value = item.TotalLoadingWeight;
            worksheet.Cell($"I{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"J{dataRow}").Value = item.TotalBillAmount;
            worksheet.Cell($"J{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"K{dataRow}").Value = item.Status;

            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range($"A{headerRow}:K{dataRow - 1}");
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Auto-size columns
        worksheet.Columns().AdjustToContents();

        // Totals
        int totalsRow = dataRow + 2;
        worksheet.Cell($"A{totalsRow}").Value = "Totals:";
        worksheet.Cell($"A{totalsRow}").Style.Font.Bold = true;

        worksheet.Cell($"A{totalsRow + 1}").Value = "Record Count:";
        worksheet.Cell($"B{totalsRow + 1}").Value = totals.RecordCount;
        worksheet.Cell($"B{totalsRow + 1}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Bills:";
        worksheet.Cell($"B{totalsRow + 2}").Value = totals.TotalBills;
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total Challans:";
        worksheet.Cell($"B{totalsRow + 3}").Value = totals.TotalChallans;
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 4}").Value = "Total Loading Weight:";
        worksheet.Cell($"B{totalsRow + 4}").Value = totals.TotalLoadingWeight;
        worksheet.Cell($"B{totalsRow + 4}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 5}").Value = "Total Gross Amount:";
        worksheet.Cell($"B{totalsRow + 5}").Value = totals.TotalGrossAmount;
        worksheet.Cell($"B{totalsRow + 5}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 6}").Value = "Average Bill Amount:";
        worksheet.Cell($"B{totalsRow + 6}").Value = totals.AverageBillAmount;
        worksheet.Cell($"B{totalsRow + 6}").Style.NumberFormat.Format = "#,##0.00";
    }

    private void ComposeDetailsWorksheet(IXLWorksheet worksheet, IReadOnlyList<PartyBillingReportDetailItem> items)
    {
        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "Party Wise Billing Report - Bill Details";
        worksheet.Cell("A2").Style.Font.Bold = true;
        worksheet.Cell("A2").Style.Font.FontSize = 12;

        worksheet.Cell("A3").Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";
        worksheet.Cell("A3").Style.Font.FontSize = 9;

        // Table Headers
        int headerRow = 5;
        worksheet.Cell($"A{headerRow}").Value = "Challan No.";
        worksheet.Cell($"B{headerRow}").Value = "Loading Date";
        worksheet.Cell($"C{headerRow}").Value = "Vehicle No.";
        worksheet.Cell($"D{headerRow}").Value = "Material";
        worksheet.Cell($"E{headerRow}").Value = "Consignor";
        worksheet.Cell($"F{headerRow}").Value = "Consignee";
        worksheet.Cell($"G{headerRow}").Value = "Source";
        worksheet.Cell($"H{headerRow}").Value = "Destination";
        worksheet.Cell($"I{headerRow}").Value = "Loading Weight";
        worksheet.Cell($"J{headerRow}").Value = "Rate";
        worksheet.Cell($"K{headerRow}").Value = "Gross Amount";

        var headerRange = worksheet.Range($"A{headerRow}:K{headerRow}");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Data
        int dataRow = headerRow + 1;
        foreach (var item in items)
        {
            worksheet.Cell($"A{dataRow}").Value = item.ChallanNumber;
            worksheet.Cell($"B{dataRow}").Value = item.LoadingDate;
            worksheet.Cell($"B{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"C{dataRow}").Value = item.VehicleNumber;
            worksheet.Cell($"D{dataRow}").Value = item.Material;
            worksheet.Cell($"E{dataRow}").Value = item.Consignor;
            worksheet.Cell($"F{dataRow}").Value = item.Consignee;
            worksheet.Cell($"G{dataRow}").Value = item.Source;
            worksheet.Cell($"H{dataRow}").Value = item.Destination;
            worksheet.Cell($"I{dataRow}").Value = item.LoadingWeight;
            worksheet.Cell($"I{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"J{dataRow}").Value = item.Rate;
            worksheet.Cell($"J{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"K{dataRow}").Value = item.GrossAmount;
            worksheet.Cell($"K{dataRow}").Style.NumberFormat.Format = "#,##0.00";

            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range($"A{headerRow}:K{dataRow - 1}");
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Auto-size columns
        worksheet.Columns().AdjustToContents();

        // Grand Totals
        int totalsRow = dataRow + 2;
        worksheet.Cell($"A{totalsRow}").Value = "Grand Totals:";
        worksheet.Cell($"A{totalsRow}").Style.Font.Bold = true;

        worksheet.Cell($"A{totalsRow + 1}").Value = "Total Records:";
        worksheet.Cell($"B{totalsRow + 1}").Value = items.Count;
        worksheet.Cell($"B{totalsRow + 1}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Loading Weight:";
        worksheet.Cell($"B{totalsRow + 2}").Value = items.Sum(x => x.LoadingWeight);
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total Gross Amount:";
        worksheet.Cell($"B{totalsRow + 3}").Value = items.Sum(x => x.GrossAmount);
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0.00";
    }
}
