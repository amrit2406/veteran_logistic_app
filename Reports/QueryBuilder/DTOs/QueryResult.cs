namespace veteran_logistic.Reports.QueryBuilder.DTOs;

/// <summary>
/// Represents the complete result of a query execution.
/// </summary>
public sealed class QueryResult
{
    /// <summary>
    /// Gets or sets the result items.
    /// </summary>
    public List<QueryResultItem> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total record count.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the execution time in milliseconds.
    /// </summary>
    public long ExecutionTimeMs { get; set; }

    /// <summary>
    /// Gets or sets the column headers (field IDs in display order).
    /// </summary>
    public List<string> ColumnHeaders { get; set; } = new();
}
