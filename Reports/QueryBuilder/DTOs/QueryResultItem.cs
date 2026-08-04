namespace veteran_logistic.Reports.QueryBuilder.DTOs;

/// <summary>
/// Represents a single result item from a query execution.
/// Uses dynamic property storage to support variable columns.
/// </summary>
public sealed class QueryResultItem
{
    /// <summary>
    /// Gets or sets the column values as a dictionary of field ID to value.
    /// </summary>
    public Dictionary<string, object?> Values { get; set; } = new();

    /// <summary>
    /// Gets a value by field ID.
    /// </summary>
    public object? GetValue(string fieldId)
    {
        return Values.TryGetValue(fieldId, out var value) ? value : null;
    }

    /// <summary>
    /// Sets a value by field ID.
    /// </summary>
    public void SetValue(string fieldId, object? value)
    {
        Values[fieldId] = value;
    }
}
