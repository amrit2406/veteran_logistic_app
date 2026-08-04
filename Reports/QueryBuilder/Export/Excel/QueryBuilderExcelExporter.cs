using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;

namespace veteran_logistic.Reports.QueryBuilder.Export.Excel;

/// <summary>
/// Implementation of the query builder Excel exporter.
/// </summary>
public sealed class QueryBuilderExcelExporter : IQueryBuilderExcelExporter
{
    private readonly ILogger<QueryBuilderExcelExporter> _logger;

    public QueryBuilderExcelExporter(ILogger<QueryBuilderExcelExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExportToExcelAsync(
        QueryResult queryResult,
        ModuleMetadata moduleMetadata,
        QueryDefinition queryDefinition,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Excel export for query builder to {FilePath}", filePath);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Query Results");

        // Header
        worksheet.Cell("A1").Value = "Veteran Logistics";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 14;

        worksheet.Cell("A2").Value = $"Query Builder - {moduleMetadata.DisplayName}";
        worksheet.Cell("A2").Style.Font.Bold = true;
        worksheet.Cell("A2").Style.Font.FontSize = 12;

        worksheet.Cell("A3").Value = $"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}";
        worksheet.Cell("A3").Style.Font.FontSize = 9;

        worksheet.Cell("A4").Value = $"Execution Time: {queryResult.ExecutionTimeMs}ms";
        worksheet.Cell("A4").Style.Font.FontSize = 9;

        worksheet.Cell("A5").Value = $"Total Records: {queryResult.TotalCount}";
        worksheet.Cell("A5").Style.Font.FontSize = 9;

        // Table Headers
        int headerRow = 7;
        int colIndex = 1;

        foreach (var columnId in queryResult.ColumnHeaders)
        {
            var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
            var headerText = field?.DisplayName ?? columnId;
            worksheet.Cell(headerRow, colIndex).Value = headerText;
            colIndex++;
        }

        var headerRange = worksheet.Range(headerRow, 1, headerRow, colIndex - 1);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Data
        int dataRow = headerRow + 1;
        foreach (var item in queryResult.Items)
        {
            colIndex = 1;
            foreach (var columnId in queryResult.ColumnHeaders)
            {
                var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
                var value = item.GetValue(columnId);
                var formattedValue = FormatValue(value, field?.DataType);
                if (formattedValue != null)
                {
                    worksheet.Cell(dataRow, colIndex).Value = formattedValue.ToString();
                }
                else
                {
                    worksheet.Cell(dataRow, colIndex).Value = "";
                }
                
                if (field?.DataType == FieldDataType.Number)
                {
                    worksheet.Cell(dataRow, colIndex).Style.NumberFormat.Format = "#,##0.00";
                }
                else if (field?.DataType == FieldDataType.Date)
                {
                    worksheet.Cell(dataRow, colIndex).Style.NumberFormat.Format = "dd-MM-yyyy";
                }
                
                colIndex++;
            }
            dataRow++;
        }

        // Data borders
        var dataRange = worksheet.Range(headerRow, 1, dataRow - 1, colIndex - 1);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        // Auto-size columns
        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(filePath);
        _logger.LogInformation("Excel export completed successfully for {RecordCount} records", queryResult.TotalCount);
    }

    private static object? FormatValue(object? value, FieldDataType? dataType)
    {
        if (value == null) return null;

        return dataType switch
        {
            FieldDataType.Boolean => value is bool b ? (b ? "Yes" : "No") : value,
            FieldDataType.Date => value is DateTime dt ? dt.ToString("dd-MM-yyyy") : value,
            FieldDataType.Number => value,
            _ => value?.ToString()
        };
    }
}
