using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;

namespace veteran_logistic.Reports.QueryBuilder.Contracts;

/// <summary>
/// Service interface for exporting query results to CSV.
/// </summary>
public interface IQueryBuilderCsvExporter
{
    /// <summary>
    /// Exports query results to a CSV file.
    /// </summary>
    /// <param name="queryResult">The query result to export.</param>
    /// <param name="moduleMetadata">The module metadata.</param>
    /// <param name="queryDefinition">The query definition.</param>
    /// <param name="filePath">The file path to save to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExportToCsvAsync(
        QueryResult queryResult,
        ModuleMetadata moduleMetadata,
        QueryDefinition queryDefinition,
        string filePath,
        CancellationToken cancellationToken = default);
}
