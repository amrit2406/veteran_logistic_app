namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents an aggregate condition in the query builder.
/// </summary>
public sealed class QueryAggregate
{
    /// <summary>
    /// Gets or sets the field ID to aggregate.
    /// </summary>
    public string FieldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the aggregate function type.
    /// </summary>
    public AggregateType AggregateType { get; set; }

    /// <summary>
    /// Gets or sets the display name for the aggregate result.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
}
