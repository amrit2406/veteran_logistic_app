using Microsoft.Extensions.Logging;
using System.Text;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;
using System.IO;

namespace veteran_logistic.Reports.QueryBuilder.Export.Csv;

/// <summary>
/// Implementation of the query builder CSV exporter.
/// </summary>
public sealed class QueryBuilderCsvExporter : IQueryBuilderCsvExporter
{
    private readonly ILogger<QueryBuilderCsvExporter> _logger;

    public QueryBuilderCsvExporter(ILogger<QueryBuilderCsvExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExportToCsvAsync(
        QueryResult queryResult,
        ModuleMetadata moduleMetadata,
        QueryDefinition queryDefinition,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting CSV export for query builder to {FilePath}", filePath);

        var csv = new StringBuilder();

        // Header row
        foreach (var columnId in queryResult.ColumnHeaders)
        {
            var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
            var headerText = field?.DisplayName ?? columnId;
            csv.Append(EscapeCsvField(headerText));
            csv.Append(',');
        }
        csv.Length--; // Remove trailing comma
        csv.AppendLine();

        // Data rows
        foreach (var item in queryResult.Items)
        {
            foreach (var columnId in queryResult.ColumnHeaders)
            {
                var field = moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
                var value = item.GetValue(columnId);
                var displayValue = FormatValue(value, field?.DataType);
                csv.Append(EscapeCsvField(displayValue));
                csv.Append(',');
            }
            csv.Length--; // Remove trailing comma
            csv.AppendLine();
        }

        await File.WriteAllTextAsync(filePath, csv.ToString(), cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("CSV export completed successfully for {RecordCount} records", queryResult.TotalCount);
    }

    private static string FormatValue(object? value, FieldDataType? dataType)
    {
        if (value == null) return string.Empty;

        return dataType switch
        {
            FieldDataType.Boolean => value is bool b ? (b ? "Yes" : "No") : value?.ToString() ?? string.Empty,
            FieldDataType.Date => value is DateTime dt ? dt.ToString("dd-MM-yyyy") : value?.ToString() ?? string.Empty,
            FieldDataType.Number => value is decimal d ? d.ToString("F2") : value?.ToString() ?? string.Empty,
            _ => value?.ToString() ?? string.Empty
        };
    }

    private static string EscapeCsvField(string? field)
    {
        if (string.IsNullOrEmpty(field))
        {
            return string.Empty;
        }

        if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }

        return field;
    }
}
