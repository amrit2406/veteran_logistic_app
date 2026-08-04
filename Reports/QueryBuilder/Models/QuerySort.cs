namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents a sort condition in the query builder.
/// </summary>
public sealed class QuerySort
{
    /// <summary>
    /// Gets or sets the field ID to sort on.
    /// </summary>
    public string FieldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sort order.
    /// </summary>
    public bool Ascending { get; set; } = true;

    /// <summary>
    /// Gets or sets the priority of this sort (lower = higher priority).
    /// </summary>
    public int Priority { get; set; }
}
