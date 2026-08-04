using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Models;

namespace veteran_logistic.Reports.QueryBuilder.Contracts;

/// <summary>
/// Service interface for executing dynamic queries in the query builder.
/// </summary>
public interface IQueryEngine
{
    /// <summary>
    /// Executes a query based on the provided definition.
    /// </summary>
    /// <param name="queryDefinition">The query definition to execute.</param>
    /// <param name="searchText">Optional global search text.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The query result.</returns>
    Task<QueryResult> ExecuteQueryAsync(
        QueryDefinition queryDefinition,
        string? searchText = null,
        CancellationToken cancellationToken = default);
}
