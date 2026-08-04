namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents a filter condition in the query builder.
/// </summary>
public sealed class QueryFilter
{
    /// <summary>
    /// Gets or sets the field ID to filter on.
    /// </summary>
    public string FieldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the filter operator.
    /// </summary>
    public FilterOperator Operator { get; set; }

    /// <summary>
    /// Gets or sets the filter value (single value).
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the second filter value (for Between operator).
    /// </summary>
    public string? Value2 { get; set; }
}
