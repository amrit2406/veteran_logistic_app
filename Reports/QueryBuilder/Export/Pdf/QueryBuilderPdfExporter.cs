using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;

namespace veteran_logistic.Reports.QueryBuilder.Export.Pdf;

/// <summary>
/// Implementation of the query builder PDF exporter.
/// </summary>
public sealed class QueryBuilderPdfExporter : IQueryBuilderPdfExporter
{
    private readonly ILogger<QueryBuilderPdfExporter> _logger;

    public QueryBuilderPdfExporter(ILogger<QueryBuilderPdfExporter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ExportToPdfAsync(
        QueryResult queryResult,
        ModuleMetadata moduleMetadata,
        QueryDefinition queryDefinition,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting PDF export for query builder to {FilePath}", filePath);

        var document = new QueryBuilderDocument(queryResult, moduleMetadata, queryDefinition);
        document.GeneratePdf(filePath);

        _logger.LogInformation("PDF export completed successfully for {RecordCount} records", queryResult.TotalCount);
    }

    private class QueryBuilderDocument : IDocument
    {
        private readonly QueryResult _queryResult;
        private readonly ModuleMetadata _moduleMetadata;
        private readonly QueryDefinition _queryDefinition;

        public QueryBuilderDocument(
            QueryResult queryResult,
            ModuleMetadata moduleMetadata,
            QueryDefinition queryDefinition)
        {
            _queryResult = queryResult;
            _moduleMetadata = moduleMetadata;
            _queryDefinition = queryDefinition;
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
                column.Item().Text($"Query Builder - {_moduleMetadata.DisplayName}").Bold().FontSize(14);
                column.Item().LineHorizontal(1);
                column.Item().Text($"Generated: {DateTime.Now:dd-MM-yyyy HH:mm}").FontSize(8);
                column.Item().Text($"Execution Time: {_queryResult.ExecutionTimeMs}ms").FontSize(8);
                column.Item().Text($"Total Records: {_queryResult.TotalCount}").FontSize(8);
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Item().Table(table =>
                {
                    var columnCount = _queryResult.ColumnHeaders.Count;
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < columnCount; i++)
                        {
                            columns.RelativeColumn(1f);
                        }
                    });

                    // Header row
                    table.Header(header =>
                    {
                        foreach (var columnId in _queryResult.ColumnHeaders)
                        {
                            var field = _moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
                            var headerText = field?.DisplayName ?? columnId;
                            header.Cell().Element(CellStyle).Text(headerText).Bold();
                        }
                    });

                    // Data rows
                    foreach (var item in _queryResult.Items)
                    {
                        foreach (var columnId in _queryResult.ColumnHeaders)
                        {
                            var field = _moduleMetadata.Fields.FirstOrDefault(f => f.FieldId == columnId);
                            var value = item.GetValue(columnId);
                            var displayValue = FormatValue(value, field?.DataType);
                            table.Cell().Element(CellStyle).Text(displayValue);
                        }
                    }
                });
            });
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
