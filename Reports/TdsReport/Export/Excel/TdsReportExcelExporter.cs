using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.TdsReport.Contracts;
using veteran_logistic.Reports.TdsReport.DTOs;

namespace veteran_logistic.Reports.TdsReport.Export.Excel;

/// <summary>
/// Implementation of the TDS report Excel exporter.
/// </summary>
public sealed class TdsReportExcelExporter : ITdsReportExcelExporter
{
    private readonly ILogger<TdsReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TdsReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public TdsReportExcelExporter(ILogger<TdsReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<TdsReportItem> items,
        TdsReportTotals totals,
        TdsReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for TDS report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        
        // Worksheet 1: TDS Transactions
        var transactionsWorksheet = workbook.Worksheets.Add("TDS Transactions");
        ComposeTransactionsWorksheet(transactionsWorksheet, items, filter, totals);

        // Worksheet 2: TDS Summary
        var summaryWorksheet = workbook.Worksheets.Add("TDS Summary");
        ComposeSummaryWorksheet(summaryWorksheet, totals);

        workbook.SaveAs(filePath);

        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private void ComposeTransactionsWorksheet(IXLWorksheet worksheet, IReadOnlyList<TdsReportItem> items, TdsReportFilter filter, TdsReportTotals totals)
    {
        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "TDS Report";
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
            
            if (filter.DateFrom.HasValue || filter.DateTo.HasValue)
            {
                filterRow++;
                worksheet.Cell($"A{filterRow}").Value = $"Date: {filter.DateFrom:dd-MM-yyyy} to {filter.DateTo:dd-MM-yyyy}";
            }
        }

        // Table Headers
        int headerRow = filter.HasFilter ? 8 : 5;
        worksheet.Cell($"A{headerRow}").Value = "Payment Date";
        worksheet.Cell($"B{headerRow}").Value = "Challan No.";
        worksheet.Cell($"C{headerRow}").Value = "Customer";
        worksheet.Cell($"D{headerRow}").Value = "Vehicle No.";
        worksheet.Cell($"E{headerRow}").Value = "Driver";
        worksheet.Cell($"F{headerRow}").Value = "Beneficiary";
        worksheet.Cell($"G{headerRow}").Value = "PAN";
        worksheet.Cell($"H{headerRow}").Value = "Bank Name";
        worksheet.Cell($"I{headerRow}").Value = "Payment Type";
        worksheet.Cell($"J{headerRow}").Value = "Challan Amount";
        worksheet.Cell($"K{headerRow}").Value = "TDS %";
        worksheet.Cell($"L{headerRow}").Value = "TDS Amount";
        worksheet.Cell($"M{headerRow}").Value = "Surcharge";
        worksheet.Cell($"N{headerRow}").Value = "Admin Charge";
        worksheet.Cell($"O{headerRow}").Value = "Net Payment";
        worksheet.Cell($"P{headerRow}").Value = "Status";

        var headerRange = worksheet.Range($"A{headerRow}:P{headerRow}");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Data
        int dataRow = headerRow + 1;
        foreach (var item in items)
        {
            worksheet.Cell($"A{dataRow}").Value = item.PaymentDate;
            worksheet.Cell($"A{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"B{dataRow}").Value = item.ChallanNumber;
            worksheet.Cell($"C{dataRow}").Value = item.Customer;
            worksheet.Cell($"D{dataRow}").Value = item.VehicleNumber;
            worksheet.Cell($"E{dataRow}").Value = item.Driver;
            worksheet.Cell($"F{dataRow}").Value = item.Beneficiary;
            worksheet.Cell($"G{dataRow}").Value = item.PAN;
            worksheet.Cell($"H{dataRow}").Value = item.BankName;
            worksheet.Cell($"I{dataRow}").Value = item.PaymentType;
            worksheet.Cell($"J{dataRow}").Value = item.ChallanAmount;
            worksheet.Cell($"J{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"K{dataRow}").Value = item.TDSPercentage;
            worksheet.Cell($"K{dataRow}").Style.NumberFormat.Format = "0.00%";
            worksheet.Cell($"L{dataRow}").Value = item.TDSAmount;
            worksheet.Cell($"L{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"M{dataRow}").Value = item.Surcharge;
            worksheet.Cell($"M{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"N{dataRow}").Value = item.AdminCharge;
            worksheet.Cell($"N{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"O{dataRow}").Value = item.NetPayment;
            worksheet.Cell($"O{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"P{dataRow}").Value = item.PaymentStatus;

            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range($"A{headerRow}:P{dataRow - 1}");
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

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Challan Amount:";
        worksheet.Cell($"B{totalsRow + 2}").Value = totals.TotalChallanAmount;
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total TDS Amount:";
        worksheet.Cell($"B{totalsRow + 3}").Value = totals.TotalTDSAmount;
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 4}").Value = "Total Surcharge:";
        worksheet.Cell($"B{totalsRow + 4}").Value = totals.TotalSurcharge;
        worksheet.Cell($"B{totalsRow + 4}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 5}").Value = "Total Admin Charge:";
        worksheet.Cell($"B{totalsRow + 5}").Value = totals.TotalAdminCharge;
        worksheet.Cell($"B{totalsRow + 5}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 6}").Value = "Total Net Payment:";
        worksheet.Cell($"B{totalsRow + 6}").Value = totals.TotalNetPayment;
        worksheet.Cell($"B{totalsRow + 6}").Style.NumberFormat.Format = "#,##0.00";
    }

    private void ComposeSummaryWorksheet(IXLWorksheet worksheet, TdsReportTotals totals)
    {
        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics - TDS Summary";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";
        worksheet.Cell("A2").Style.Font.FontSize = 9;

        worksheet.Cell("A4").Value = "Tax Summary";
        worksheet.Cell("A4").Style.Font.Bold = true;
        worksheet.Cell("A4").Style.Font.FontSize = 12;

        // Summary Data
        int row = 6;
        worksheet.Cell($"A{row}").Value = "Total Challan Amount";
        worksheet.Cell($"B{row}").Value = totals.TotalChallanAmount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Total TDS Amount";
        worksheet.Cell($"B{row}").Value = totals.TotalTDSAmount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Total Surcharge";
        worksheet.Cell($"B{row}").Value = totals.TotalSurcharge;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Total Admin Charge";
        worksheet.Cell($"B{row}").Value = totals.TotalAdminCharge;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Total Net Payment";
        worksheet.Cell($"B{row}").Value = totals.TotalNetPayment;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "";
        row++;

        worksheet.Cell($"A{row}").Value = "TDS Statistics";
        worksheet.Cell($"A{row}").Style.Font.Bold = true;
        worksheet.Cell($"A{row}").Style.Font.FontSize = 12;
        row++;

        worksheet.Cell($"A{row}").Value = "Record Count";
        worksheet.Cell($"B{row}").Value = totals.RecordCount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0";
        row++;

        worksheet.Cell($"A{row}").Value = "Average TDS Amount";
        worksheet.Cell($"B{row}").Value = totals.AverageTDSAmount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Highest TDS Amount"
;
        worksheet.Cell($"B{row}").Value = totals.HighestTDSAmount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        worksheet.Cell($"A{row}").Value = "Lowest TDS Amount";
        worksheet.Cell($"B{row}").Value = totals.LowestTDSAmount;
        worksheet.Cell($"B{row}").Style.NumberFormat.Format = "#,##0.00";
        row++;

        // Format the summary table
        var summaryRange = worksheet.Range($"A6:B{row}");
        summaryRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        summaryRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Auto-size columns
        worksheet.Columns().AdjustToContents();
    }
}
