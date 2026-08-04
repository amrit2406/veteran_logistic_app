namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents aggregate function types available in the query builder.
/// </summary>
public enum AggregateType
{
    /// <summary>
    /// Count aggregate function.
    /// </summary>
    Count,

    /// <summary>
    /// Sum aggregate function.
    /// </summary>
    Sum,

    /// <summary>
    /// Average aggregate function.
    /// </summary>
    Average,

    /// <summary>
    /// Minimum aggregate function.
    /// </summary>
    Minimum,

    /// <summary>
    /// Maximum aggregate function.
    /// </summary>
    Maximum
}
