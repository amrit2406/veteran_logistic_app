using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.PaymentReport.Contracts;
using veteran_logistic.Reports.PaymentReport.DTOs;

namespace veteran_logistic.Reports.PaymentReport.Export.Excel;

/// <summary>
/// Implementation of the payment report Excel exporter.
/// </summary>
public sealed class PaymentReportExcelExporter : IPaymentReportExcelExporter
{
    private readonly ILogger<PaymentReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public PaymentReportExcelExporter(ILogger<PaymentReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<PaymentReportItem> items,
        PaymentReportTotals totals,
        PaymentReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for payment report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Payment Report");

        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = "Payment Report";
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
        worksheet.Cell($"C{headerRow}").Value = "TP Number";
        worksheet.Cell($"D{headerRow}").Value = "Vehicle No.";
        worksheet.Cell($"E{headerRow}").Value = "Loading Date";
        worksheet.Cell($"F{headerRow}").Value = "Unloading Date";
        worksheet.Cell($"G{headerRow}").Value = "Customer";
        worksheet.Cell($"H{headerRow}").Value = "Material";
        worksheet.Cell($"I{headerRow}").Value = "Driver";
        worksheet.Cell($"J{headerRow}").Value = "Vehicle Owner";
        worksheet.Cell($"K{headerRow}").Value = "Payment Location";
        worksheet.Cell($"L{headerRow}").Value = "Payment Type";
        worksheet.Cell($"M{headerRow}").Value = "Beneficiary";
        worksheet.Cell($"N{headerRow}").Value = "PAN";
        worksheet.Cell($"O{headerRow}").Value = "Bank Name";
        worksheet.Cell($"P{headerRow}").Value = "Account Number";
        worksheet.Cell($"Q{headerRow}").Value = "IFSC Code";
        worksheet.Cell($"R{headerRow}").Value = "UTR Number";
        worksheet.Cell($"S{headerRow}").Value = "Mobile Number";
        worksheet.Cell($"T{headerRow}").Value = "Loading Weight";
        worksheet.Cell($"U{headerRow}").Value = "Unloading Weight";
        worksheet.Cell($"V{headerRow}").Value = "Driver Commission";
        worksheet.Cell($"W{headerRow}").Value = "Challan Amount";
        worksheet.Cell($"X{headerRow}").Value = "TDS Amount";
        worksheet.Cell($"Y{headerRow}").Value = "Surcharge Amount";
        worksheet.Cell($"Z{headerRow}").Value = "Admin Charge";
        worksheet.Cell($"AA{headerRow}").Value = "Net Payment";
        worksheet.Cell($"AB{headerRow}").Value = "Notes";
        worksheet.Cell($"AC{headerRow}").Value = "Payment Status";
        worksheet.Cell($"AD{headerRow}").Value = "Active";

        var headerRange = worksheet.Range($"A{headerRow}:AD{headerRow}");
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
            worksheet.Cell($"C{dataRow}").Value = item.TPNumber;
            worksheet.Cell($"D{dataRow}").Value = item.VehicleNumber;
            worksheet.Cell($"E{dataRow}").Value = item.LoadingDate;
            worksheet.Cell($"E{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"F{dataRow}").Value = item.UnloadingDate;
            worksheet.Cell($"F{dataRow}").Style.NumberFormat.Format = "dd-MM-yyyy";
            worksheet.Cell($"G{dataRow}").Value = item.CustomerName;
            worksheet.Cell($"H{dataRow}").Value = item.MaterialName;
            worksheet.Cell($"I{dataRow}").Value = item.Driver;
            worksheet.Cell($"J{dataRow}").Value = item.VehicleOwner;
            worksheet.Cell($"K{dataRow}").Value = item.PaymentLocationName;
            worksheet.Cell($"L{dataRow}").Value = item.PaymentType;
            worksheet.Cell($"M{dataRow}").Value = item.Beneficiary;
            worksheet.Cell($"N{dataRow}").Value = item.PAN;
            worksheet.Cell($"O{dataRow}").Value = item.BankName;
            worksheet.Cell($"P{dataRow}").Value = item.AccountNumber;
            worksheet.Cell($"Q{dataRow}").Value = item.IFSCCode;
            worksheet.Cell($"R{dataRow}").Value = item.UTRNumber;
            worksheet.Cell($"S{dataRow}").Value = item.MobileNumber;
            worksheet.Cell($"T{dataRow}").Value = item.LoadingWeight;
            worksheet.Cell($"T{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"U{dataRow}").Value = item.UnloadingWeight;
            worksheet.Cell($"U{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"V{dataRow}").Value = item.DriverCommission;
            worksheet.Cell($"V{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"W{dataRow}").Value = item.ChallanAmount;
            worksheet.Cell($"W{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"X{dataRow}").Value = item.TDSAmount;
            worksheet.Cell($"X{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"Y{dataRow}").Value = item.SurchargeAmount;
            worksheet.Cell($"Y{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"Z{dataRow}").Value = item.AdminCharge;
            worksheet.Cell($"Z{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"AA{dataRow}").Value = item.NetPayment;
            worksheet.Cell($"AA{dataRow}").Style.NumberFormat.Format = "#,##0.00";
            worksheet.Cell($"AB{dataRow}").Value = item.Notes;
            worksheet.Cell($"AC{dataRow}").Value = item.PaymentStatus;
            worksheet.Cell($"AD{dataRow}").Value = item.IsActive ? "Yes" : "No";

            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range($"A{headerRow}:AD{dataRow - 1}");
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

        worksheet.Cell($"A{totalsRow + 2}").Value = "Total Loading Weight:";
        worksheet.Cell($"B{totalsRow + 2}").Value = totals.TotalLoadingWeight;
        worksheet.Cell($"B{totalsRow + 2}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 3}").Value = "Total Unloading Weight:";
        worksheet.Cell($"B{totalsRow + 3}").Value = totals.TotalUnloadingWeight;
        worksheet.Cell($"B{totalsRow + 3}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 4}").Value = "Total Driver Commission:";
        worksheet.Cell($"B{totalsRow + 4}").Value = totals.TotalDriverCommission;
        worksheet.Cell($"B{totalsRow + 4}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 5}").Value = "Total Challan Amount:";
        worksheet.Cell($"B{totalsRow + 5}").Value = totals.TotalChallanAmount;
        worksheet.Cell($"B{totalsRow + 5}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 6}").Value = "Total TDS Amount:";
        worksheet.Cell($"B{totalsRow + 6}").Value = totals.TotalTDSAmount;
        worksheet.Cell($"B{totalsRow + 6}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 7}").Value = "Total Surcharge Amount:";
        worksheet.Cell($"B{totalsRow + 7}").Value = totals.TotalSurchargeAmount;
        worksheet.Cell($"B{totalsRow + 7}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 8}").Value = "Total Admin Charge:";
        worksheet.Cell($"B{totalsRow + 8}").Value = totals.TotalAdminCharge;
        worksheet.Cell($"B{totalsRow + 8}").Style.NumberFormat.Format = "#,##0.00";

        worksheet.Cell($"A{totalsRow + 9}").Value = "Total Net Payment:";
        worksheet.Cell($"B{totalsRow + 9}").Value = totals.TotalNetPayment;
        worksheet.Cell($"B{totalsRow + 9}").Style.NumberFormat.Format = "#,##0.00";

        workbook.SaveAs(filePath);

        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }
}
