using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.DOStatusReport.Contracts;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Export.Excel;

/// <summary>
/// Implementation of the DO status report Excel exporter.
/// </summary>
public sealed class DOStatusReportExcelExporter : IDOStatusReportExcelExporter
{
    private readonly ILogger<DOStatusReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DOStatusReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public DOStatusReportExcelExporter(ILogger<DOStatusReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<DOStatusReportItem> items,
        DOStatusReportSummaryCards summaryCards,
        DOStatusReportTotals totals,
        DOStatusReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for DO status report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("DO Status Report");

        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "DO Status Report";
        worksheet.Cell("A2").Style.Font.Bold = true;
        worksheet.Cell("A2").Style.Font.FontSize = 12;

        worksheet.Cell("A3").Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";
        worksheet.Cell("A3").Style.Font.FontSize = 9;

        // Summary Cards
        int summaryRow = 5;
        worksheet.Cell($"A{summaryRow}").Value = "Summary:";
        worksheet.Cell($"A{summaryRow}").Style.Font.Bold = true;

        worksheet.Cell($"A{summaryRow + 1}").Value = "Total DO:";
        worksheet.Cell($"B{summaryRow + 1}").Value = summaryCards.TotalDO;
        worksheet.Cell($"A{summaryRow + 2}").Value = "Today's Loading:";
        worksheet.Cell($"B{summaryRow + 2}").Value = summaryCards.TodayLoading;
        worksheet.Cell($"A{summaryRow + 3}").Value = "Today's Completed:";
        worksheet.Cell($"B{summaryRow + 3}").Value = summaryCards.TodayCompleted;
        worksheet.Cell($"A{summaryRow + 4}").Value = "Running DO:";
        worksheet.Cell($"B{summaryRow + 4}").Value = summaryCards.RunningDO;
        worksheet.Cell($"A{summaryRow + 5}").Value = "Completed DO:";
        worksheet.Cell($"B{summaryRow + 5}").Value = summaryCards.CompletedDO;
        worksheet.Cell($"A{summaryRow + 6}").Value = "Payment Pending:";
        worksheet.Cell($"B{summaryRow + 6}").Value = summaryCards.PaymentPending;
        worksheet.Cell($"A{summaryRow + 7}").Value = "Bill Pending:";
        worksheet.Cell($"B{summaryRow + 7}").Value = summaryCards.BillPending;
        worksheet.Cell($"A{summaryRow + 8}").Value = "Delayed DO:";
        worksheet.Cell($"B{summaryRow + 8}").Value = summaryCards.DelayedDO;
        worksheet.Cell($"A{summaryRow + 9}").Value = "Exception DO:";
        worksheet.Cell($"B{summaryRow + 9}").Value = summaryCards.ExceptionDO;
        worksheet.Cell($"A{summaryRow + 10}").Value = "Completion %:";
        worksheet.Cell($"B{summaryRow + 10}").Value = summaryCards.CompletionPercentage;
        worksheet.Cell($"B{summaryRow + 10}").Style.NumberFormat.Format = "0.00";
        worksheet.Cell($"A{summaryRow + 11}").Value = "Pending %:";
        worksheet.Cell($"B{summaryRow + 11}").Value = summaryCards.PendingPercentage;
        worksheet.Cell($"B{summaryRow + 11}").Style.NumberFormat.Format = "0.00";

        // Filters
        if (filter.HasFilter)
        {
            int filterRow = summaryRow + 8;
            worksheet.Cell($"A{filterRow}").Value = "Filters Applied:";
            worksheet.Cell($"A{filterRow}").Style.Font.Bold = true;
            
            if (filter.DateFrom.HasValue || filter.DateTo.HasValue)
            {
                filterRow++;
                worksheet.Cell($"A{filterRow}").Value = $"Date: {filter.DateFrom:dd-MM-yyyy} to {filter.DateTo:dd-MM-yyyy}";
            }
        }

        // Table Headers
        int headerRow = filter.HasFilter ? summaryRow + 14 : summaryRow + 11;
        worksheet.Cell($"A{headerRow}").Value = "Challan No.";
        worksheet.Cell($"B{headerRow}").Value = "TP Number";
        worksheet.Cell($"C{headerRow}").Value = "Loading Date";
        worksheet.Cell($"D{headerRow}").Value = "Vehicle No.";
        worksheet.Cell($"E{headerRow}").Value = "Consignor";
        worksheet.Cell($"F{headerRow}").Value = "Consignee";
        worksheet.Cell($"G{headerRow}").Value = "Driver";
        worksheet.Cell($"H{headerRow}").Value = "Material";
        worksheet.Cell($"I{headerRow}").Value = "Loading Weight";
        worksheet.Cell($"J{headerRow}").Value = "Unloading Weight";
        worksheet.Cell($"K{headerRow}").Value = "Shortage Weight";
        worksheet.Cell($"L{headerRow}").Value = "Gross Amount";
        worksheet.Cell($"M{headerRow}").Value = "Challan Money";
        worksheet.Cell($"N{headerRow}").Value = "Pending Amount";
        worksheet.Cell($"O{headerRow}").Value = "Bill Number";
        worksheet.Cell($"P{headerRow}").Value = "Bill Date";
        worksheet.Cell($"Q{headerRow}").Value = "Age (Days)";
        worksheet.Cell($"R{headerRow}").Value = "Delayed";
        worksheet.Cell($"S{headerRow}").Value = "DO Status";
        worksheet.Cell($"T{headerRow}").Value = "Payment Status";
        worksheet.Cell($"U{headerRow}").Value = "Billing Status";
        worksheet.Cell($"V{headerRow}").Value = "Exception";

        var headerRange = worksheet.Range($"A{headerRow}:V{headerRow}");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Data
        int dataRow = headerRow + 1;
        foreach (var item in items)
        {
            worksheet.Cell($"A{dataRow}").Value = item.ChallanNumber;
            worksheet.Cell($"B{dataRow}").Value = item.TPNumber;
            worksheet.Cell($"C{dataRow}").Value = item.LoadingDate;
            worksheet.Cell($"C{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"D{dataRow}").Value = item.VehicleNumber;
            worksheet.Cell($"E{dataRow}").Value = item.ConsignorName;
            worksheet.Cell($"F{dataRow}").Value = item.ConsigneeName;
            worksheet.Cell($"G{dataRow}").Value = item.Driver;
            worksheet.Cell($"H{dataRow}").Value = item.MaterialName;
            worksheet.Cell($"I{dataRow}").Value = item.LoadingWeight;
            worksheet.Cell($"I{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"J{dataRow}").Value = item.UnloadingWeight;
            worksheet.Cell($"J{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"K{dataRow}").Value = item.ShortageWeight;
            worksheet.Cell($"K{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"L{dataRow}").Value = item.GrossAmount;
            worksheet.Cell($"L{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"M{dataRow}").Value = item.ChallanMoney;
            worksheet.Cell($"M{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"N{dataRow}").Value = item.PendingAmount;
            worksheet.Cell($"N{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"O{dataRow}").Value = item.BillNumber;
            worksheet.Cell($"P{dataRow}").Value = item.BillDate;
            worksheet.Cell($"P{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"Q{dataRow}").Value = item.AgeInDays;
            worksheet.Cell($"R{dataRow}").Value = item.IsDelayed ? "Yes" : "No";
            worksheet.Cell($"S{dataRow}").Value = item.DOStatus.ToString();
            worksheet.Cell($"T{dataRow}").Value = item.PaymentStatus.ToString();
            worksheet.Cell($"U{dataRow}").Value = item.BillingStatus.ToString();
            worksheet.Cell($"V{dataRow}").Value = item.ExceptionType.ToString();

            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range($"A{headerRow}:V{dataRow - 1}");
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Auto-size columns
        worksheet.Columns().AdjustToContents();

        // Totals
        int totalsRow = dataRow + 2;
        worksheet.Cell($"A{totalsRow}").Value = "Totals:";
        worksheet.Cell($"A{totalsRow}").Style.Font.Bold = true;

        worksheet.Cell($"A{totalsRow + 1}").Value = "Record Count:";
        worksheet.Cell($"B{totalsRow + 1}").Value = totals.TotalRecords;
        worksheet.Cell($"B{totalsRow + 1}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Loading Weight:";
        worksheet.Cell($"B{totalsRow + 2}").Value = totals.TotalLoadingWeight;
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total Unloading Weight:";
        worksheet.Cell($"B{totalsRow + 3}").Value = totals.TotalUnloadingWeight;
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 4}").Value = "Total Shortage Weight:";
        worksheet.Cell($"B{totalsRow + 4}").Value = totals.TotalShortageWeight;
        worksheet.Cell($"B{totalsRow + 4}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 5}").Value = "Total Gross Amount:";
        worksheet.Cell($"B{totalsRow + 5}").Value = totals.TotalGrossAmount;
        worksheet.Cell($"B{totalsRow + 5}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 6}").Value = "Total Challan Money:";
        worksheet.Cell($"B{totalsRow + 6}").Value = totals.TotalChallanMoney;
        worksheet.Cell($"B{totalsRow + 6}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 7}").Value = "Total Pending Amount:";
        worksheet.Cell($"B{totalsRow + 7}").Value = totals.TotalPendingAmount;
        worksheet.Cell($"B{totalsRow + 7}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 8}").Value = "Completed Gross Amount:";
        worksheet.Cell($"B{totalsRow + 8}").Value = totals.CompletedGrossAmount;
        worksheet.Cell($"B{totalsRow + 8}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 9}").Value = "Pending Gross Amount:";
        worksheet.Cell($"B{totalsRow + 9}").Value = totals.PendingGrossAmount;
        worksheet.Cell($"B{totalsRow + 9}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 10}").Value = "Today's Gross Amount:";
        worksheet.Cell($"B{totalsRow + 10}").Value = totals.TodayGrossAmount;
        worksheet.Cell($"B{totalsRow + 10}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 11}").Value = "Today's Loading Weight:";
        worksheet.Cell($"B{totalsRow + 11}").Value = totals.TodayLoadingWeight;
        worksheet.Cell($"B{totalsRow + 11}").Style.NumberFormat.Format = "#,##0.00";

        workbook.SaveAs(filePath);
        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }
}
