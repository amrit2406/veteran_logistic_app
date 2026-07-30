using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.LoadingReport.Contracts;
using veteran_logistic.Reports.LoadingReport.DTOs;

namespace veteran_logistic.Reports.LoadingReport.Export.Excel;

/// <summary>
/// Implementation of the loading report Excel exporter.
/// </summary>
public sealed class LoadingReportExcelExporter : ILoadingReportExcelExporter
{
    private readonly ILogger<LoadingReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public LoadingReportExcelExporter(ILogger<LoadingReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<LoadingReportItem> items,
        LoadingReportTotals totals,
        LoadingReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for loading report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Loading Report");

        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "Loading Report";
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
        worksheet.Cell($"A{headerRow}").Value = "Challan No.";
        worksheet.Cell($"B{headerRow}").Value = "Loading Date";
        worksheet.Cell($"C{headerRow}").Value = "TP Number";
        worksheet.Cell($"D{headerRow}").Value = "Vehicle No.";
        worksheet.Cell($"E{headerRow}").Value = "Consignor";
        worksheet.Cell($"F{headerRow}").Value = "Consignee";
        worksheet.Cell($"G{headerRow}").Value = "Source";
        worksheet.Cell($"H{headerRow}").Value = "Destination";
        worksheet.Cell($"I{headerRow}").Value = "Material";
        worksheet.Cell($"J{headerRow}").Value = "Driver";
        worksheet.Cell($"K{headerRow}").Value = "Gross Weight";
        worksheet.Cell($"L{headerRow}").Value = "Tare Weight";
        worksheet.Cell($"M{headerRow}").Value = "Loading Weight";
        worksheet.Cell($"N{headerRow}").Value = "Rate";
        worksheet.Cell($"O{headerRow}").Value = "Gross Amount";
        worksheet.Cell($"P{headerRow}").Value = "Fuel Amount";
        worksheet.Cell($"Q{headerRow}").Value = "Cash Advance";
        worksheet.Cell($"R{headerRow}").Value = "Other Advance";
        worksheet.Cell($"S{headerRow}").Value = "Payment Location";
        worksheet.Cell($"T{headerRow}").Value = "Union/Vendor";
        worksheet.Cell($"U{headerRow}").Value = "Owner";
        worksheet.Cell($"V{headerRow}").Value = "Active";

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
            worksheet.Cell($"B{dataRow}").Value = item.LoadingDate;
            worksheet.Cell($"B{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"C{dataRow}").Value = item.TPNumber;
            worksheet.Cell($"D{dataRow}").Value = item.VehicleNumber;
            worksheet.Cell($"E{dataRow}").Value = item.ConsignorName;
            worksheet.Cell($"F{dataRow}").Value = item.ConsigneeName;
            worksheet.Cell($"G{dataRow}").Value = item.SourceName;
            worksheet.Cell($"H{dataRow}").Value = item.DestinationName;
            worksheet.Cell($"I{dataRow}").Value = item.MaterialName;
            worksheet.Cell($"J{dataRow}").Value = item.Driver;
            worksheet.Cell($"K{dataRow}").Value = item.GrossWeight;
            worksheet.Cell($"K{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"L{dataRow}").Value = item.TareWeight;
            worksheet.Cell($"L{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"M{dataRow}").Value = item.LoadingWeight;
            worksheet.Cell($"M{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"N{dataRow}").Value = item.Rate;
            worksheet.Cell($"N{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"O{dataRow}").Value = item.GrossAmount;
            worksheet.Cell($"O{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"P{dataRow}").Value = item.FuelAmount;
            worksheet.Cell($"P{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"Q{dataRow}").Value = item.CashAdvance;
            worksheet.Cell($"Q{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"R{dataRow}").Value = item.OtherAdvance;
            worksheet.Cell($"R{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"S{dataRow}").Value = item.PaymentLocationName;
            worksheet.Cell($"T{dataRow}").Value = item.UnionVendorName;
            worksheet.Cell($"U{dataRow}").Value = item.OwnerName;
            worksheet.Cell($"V{dataRow}").Value = item.IsActive ? "Yes" : "No";

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
        worksheet.Cell($"B{totalsRow + 1}").Value = totals.RecordCount;
        worksheet.Cell($"B{totalsRow + 1}").Style.NumberFormat.Format = "#,##0";

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Gross Weight:";
        worksheet.Cell($"B{totalsRow + 2}").Value = totals.TotalGrossWeight;
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total Tare Weight:";
        worksheet.Cell($"B{totalsRow + 3}").Value = totals.TotalTareWeight;
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 4}").Value = "Total Loading Weight:";
        worksheet.Cell($"B{totalsRow + 4}").Value = totals.TotalLoadingWeight;
        worksheet.Cell($"B{totalsRow + 4}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 5}").Value = "Total Gross Amount:";
        worksheet.Cell($"B{totalsRow + 5}").Value = totals.TotalGrossAmount;
        worksheet.Cell($"B{totalsRow + 5}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 6}").Value = "Total Fuel Amount:";
        worksheet.Cell($"B{totalsRow + 6}").Value = totals.TotalFuelAmount;
        worksheet.Cell($"B{totalsRow + 6}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 7}").Value = "Total Cash Advance:";
        worksheet.Cell($"B{totalsRow + 7}").Value = totals.TotalCashAdvance;
        worksheet.Cell($"B{totalsRow + 7}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 8}").Value = "Total Other Advance:";
        worksheet.Cell($"B{totalsRow + 8}").Value = totals.TotalOtherAdvance;
        worksheet.Cell($"B{totalsRow + 8}").Style.NumberFormat.Format = "#,##0.00";

        workbook.SaveAs(filePath);

        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }
}
