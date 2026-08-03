using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.ConsolidatedReport.Contracts;
using veteran_logistic.Reports.ConsolidatedReport.DTOs;

namespace veteran_logistic.Reports.ConsolidatedReport.Export.Excel;

/// <summary>
/// Implementation of the consolidated report Excel exporter.
/// </summary>
public sealed class ConsolidatedReportExcelExporter : IConsolidatedReportExcelExporter
{
    private readonly ILogger<ConsolidatedReportExcelExporter> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsolidatedReportExcelExporter"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public ConsolidatedReportExcelExporter(ILogger<ConsolidatedReportExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task ExportToExcelAsync(
        IReadOnlyList<ConsolidatedReportItem> items,
        ConsolidatedReportTotals totals,
        ConsolidatedReportSummaryCards summaryCards,
        ConsolidatedReportFilter filter,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for consolidated report to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        
        // Worksheet 1: Consolidated Transactions
        var transactionsSheet = workbook.Worksheets.Add("Consolidated Transactions");
        AddTransactionsWorksheet(transactionsSheet, items, filter);

        // Worksheet 2: Summary
        var summarySheet = workbook.Worksheets.Add("Summary");
        AddSummaryWorksheet(summarySheet, summaryCards, totals);

        // Worksheet 3: Lifecycle Statistics
        var lifecycleSheet = workbook.Worksheets.Add("Lifecycle Statistics");
        AddLifecycleWorksheet(lifecycleSheet, summaryCards);

        workbook.SaveAs(filePath);

        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", items.Count);
        await Task.CompletedTask;
    }

    private void AddTransactionsWorksheet(IXLWorksheet worksheet, IReadOnlyList<ConsolidatedReportItem> items, ConsolidatedReportFilter filter)
    {
        // Add headers
        var headers = new[]
        {
            "Challan", "Loading Date", "TP Number", "Vehicle", "Material", "Consignor", "Consignee",
            "Source", "Destination", "Loading Weight", "Rate", "Loading Amount",
            "Unloading Date", "Unloading Weight", "Shortage Weight",
            "Payment Date", "Beneficiary", "Payment Type", "Driver Commission", "Challan Amount",
            "TDS Amount", "Surcharge", "Admin Charge", "Net Payment", "Payment Status",
            "Bill Number", "Bill Date", "Customer", "Third Party", "Permit Number", "Billing Status",
            "Driver", "Owner", "Company", "Payment Location", "Lifecycle Status"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        // Add data
        int row = 2;
        foreach (var item in items)
        {
            worksheet.Cell(row, 1).Value = item.ChallanNumber;
            worksheet.Cell(row, 2).Value = item.LoadingDate.ToString("dd-MM-yyyy");
            worksheet.Cell(row, 3).Value = item.TPNumber;
            worksheet.Cell(row, 4).Value = item.VehicleNumber;
            worksheet.Cell(row, 5).Value = item.MaterialName;
            worksheet.Cell(row, 6).Value = item.ConsignorName;
            worksheet.Cell(row, 7).Value = item.ConsigneeName;
            worksheet.Cell(row, 8).Value = item.SourceName;
            worksheet.Cell(row, 9).Value = item.DestinationName;
            worksheet.Cell(row, 10).Value = item.LoadingWeight;
            worksheet.Cell(row, 11).Value = item.Rate;
            worksheet.Cell(row, 12).Value = item.LoadingAmount;
            worksheet.Cell(row, 13).Value = item.UnloadingDate?.ToString("dd-MM-yyyy");
            worksheet.Cell(row, 14).Value = item.UnloadingWeight;
            worksheet.Cell(row, 15).Value = item.ShortageWeight;
            worksheet.Cell(row, 16).Value = item.PaymentDate?.ToString("dd-MM-yyyy");
            worksheet.Cell(row, 17).Value = item.Beneficiary;
            worksheet.Cell(row, 18).Value = item.PaymentType;
            worksheet.Cell(row, 19).Value = item.DriverCommission;
            worksheet.Cell(row, 20).Value = item.ChallanAmount;
            worksheet.Cell(row, 21).Value = item.TDSAmount;
            worksheet.Cell(row, 22).Value = item.Surcharge;
            worksheet.Cell(row, 23).Value = item.AdminCharge;
            worksheet.Cell(row, 24).Value = item.NetPayment;
            worksheet.Cell(row, 25).Value = item.PaymentStatus;
            worksheet.Cell(row, 26).Value = item.BillNumber;
            worksheet.Cell(row, 27).Value = item.BillDate?.ToString("dd-MM-yyyy");
            worksheet.Cell(row, 28).Value = item.CustomerName;
            worksheet.Cell(row, 29).Value = item.ThirdParty;
            worksheet.Cell(row, 30).Value = item.PermitNumber;
            worksheet.Cell(row, 31).Value = item.BillingStatus;
            worksheet.Cell(row, 32).Value = item.Driver;
            worksheet.Cell(row, 33).Value = item.OwnerName;
            worksheet.Cell(row, 34).Value = item.CompanyName;
            worksheet.Cell(row, 35).Value = item.PaymentLocationName;
            worksheet.Cell(row, 36).Value = item.LifecycleStatus;
            row++;
        }

        // Auto-size columns
        worksheet.Columns().AdjustToContents();

        // Add filters
        worksheet.Range(1, 1, 1, headers.Length).SetAutoFilter();
    }

    private void AddSummaryWorksheet(IXLWorksheet worksheet, ConsolidatedReportSummaryCards summaryCards, ConsolidatedReportTotals totals)
    {
        // Summary Cards section
        worksheet.Cell(1, 1).Value = "Summary Cards";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;

        worksheet.Cell(3, 1).Value = "Total Transactions";
        worksheet.Cell(3, 2).Value = summaryCards.TotalTransactions;
        worksheet.Cell(4, 1).Value = "Loading Only";
        worksheet.Cell(4, 2).Value = summaryCards.LoadingOnly;
        worksheet.Cell(5, 1).Value = "Pending Unloading";
        worksheet.Cell(5, 2).Value = summaryCards.PendingUnloading;
        worksheet.Cell(6, 1).Value = "Pending Payment";
        worksheet.Cell(6, 2).Value = summaryCards.PendingPayment;
        worksheet.Cell(7, 1).Value = "Pending Billing";
        worksheet.Cell(7, 2).Value = summaryCards.PendingBilling;
        worksheet.Cell(8, 1).Value = "Completed";
        worksheet.Cell(8, 2).Value = summaryCards.Completed;
        worksheet.Cell(9, 1).Value = "Total Revenue";
        worksheet.Cell(9, 2).Value = summaryCards.TotalRevenue;
        worksheet.Cell(10, 1).Value = "Total Net Payment";
        worksheet.Cell(10, 2).Value = summaryCards.TotalNetPayment;
        worksheet.Cell(11, 1).Value = "Total TDS";
        worksheet.Cell(11, 2).Value = summaryCards.TotalTDS;

        // Totals section
        worksheet.Cell(14, 1).Value = "Totals";
        worksheet.Cell(14, 1).Style.Font.Bold = true;
        worksheet.Cell(14, 1).Style.Font.FontSize = 14;

        worksheet.Cell(16, 1).Value = "Record Count";
        worksheet.Cell(16, 2).Value = totals.RecordCount;
        worksheet.Cell(17, 1).Value = "Total Loading Weight";
        worksheet.Cell(17, 2).Value = totals.TotalLoadingWeight;
        worksheet.Cell(18, 1).Value = "Total Unloading Weight";
        worksheet.Cell(18, 2).Value = totals.TotalUnloadingWeight;
        worksheet.Cell(19, 1).Value = "Total Shortage Weight";
        worksheet.Cell(19, 2).Value = totals.TotalShortageWeight;
        worksheet.Cell(20, 1).Value = "Total Loading Amount";
        worksheet.Cell(20, 2).Value = totals.TotalLoadingAmount;
        worksheet.Cell(21, 1).Value = "Total Challan Amount";
        worksheet.Cell(21, 2).Value = totals.TotalChallanAmount;
        worksheet.Cell(22, 1).Value = "Total Net Payment";
        worksheet.Cell(22, 2).Value = totals.TotalNetPayment;
        worksheet.Cell(23, 1).Value = "Total TDS Amount";
        worksheet.Cell(23, 2).Value = totals.TotalTDSAmount;
        worksheet.Cell(24, 1).Value = "Total Bills";
        worksheet.Cell(24, 2).Value = totals.TotalBills;
        worksheet.Cell(25, 1).Value = "Average Net Payment";
        worksheet.Cell(25, 2).Value = totals.AverageNetPayment;

        // Format numeric columns
        worksheet.Column(2).Style.NumberFormat.Format = "#,##0.00";

        worksheet.Columns().AdjustToContents();
    }

    private void AddLifecycleWorksheet(IXLWorksheet worksheet, ConsolidatedReportSummaryCards summaryCards)
    {
        // Header
        worksheet.Cell(1, 1).Value = "Lifecycle Statistics";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;

        // Lifecycle breakdown
        worksheet.Cell(3, 1).Value = "Lifecycle Stage";
        worksheet.Cell(3, 2).Value = "Count";
        worksheet.Cell(3, 3).Value = "Percentage";
        worksheet.Cell(3, 1).Style.Font.Bold = true;
        worksheet.Cell(3, 2).Style.Font.Bold = true;
        worksheet.Cell(3, 3).Style.Font.Bold = true;

        worksheet.Cell(4, 1).Value = "Loading Only";
        worksheet.Cell(4, 2).Value = summaryCards.LoadingOnly;
        worksheet.Cell(4, 3).Value = summaryCards.TotalTransactions > 0 ? 
            (summaryCards.LoadingOnly * 100.0 / summaryCards.TotalTransactions).ToString("F2") + "%" : "0%";

        worksheet.Cell(5, 1).Value = "Pending Unloading";
        worksheet.Cell(5, 2).Value = summaryCards.PendingUnloading;
        worksheet.Cell(5, 3).Value = summaryCards.TotalTransactions > 0 ? 
            (summaryCards.PendingUnloading * 100.0 / summaryCards.TotalTransactions).ToString("F2") + "%" : "0%";

        worksheet.Cell(6, 1).Value = "Pending Payment";
        worksheet.Cell(6, 2).Value = summaryCards.PendingPayment;
        worksheet.Cell(6, 3).Value = summaryCards.TotalTransactions > 0 ? 
            (summaryCards.PendingPayment * 100.0 / summaryCards.TotalTransactions).ToString("F2") + "%" : "0%";

        worksheet.Cell(7, 1).Value = "Pending Billing";
        worksheet.Cell(7, 2).Value = summaryCards.PendingBilling;
        worksheet.Cell(7, 3).Value = summaryCards.TotalTransactions > 0 ? 
            (summaryCards.PendingBilling * 100.0 / summaryCards.TotalTransactions).ToString("F2") + "%" : "0%";

        worksheet.Cell(8, 1).Value = "Completed";
        worksheet.Cell(8, 2).Value = summaryCards.Completed;
        worksheet.Cell(8, 3).Value = summaryCards.TotalTransactions > 0 ? 
            (summaryCards.Completed * 100.0 / summaryCards.TotalTransactions).ToString("F2") + "%" : "0%";

        worksheet.Cell(9, 1).Value = "Total";
        worksheet.Cell(9, 2).Value = summaryCards.TotalTransactions;
        worksheet.Cell(9, 3).Value = "100%";
        worksheet.Cell(9, 1).Style.Font.Bold = true;
        worksheet.Cell(9, 2).Style.Font.Bold = true;
        worksheet.Cell(9, 3).Style.Font.Bold = true;

        worksheet.Columns().AdjustToContents();
    }
}
