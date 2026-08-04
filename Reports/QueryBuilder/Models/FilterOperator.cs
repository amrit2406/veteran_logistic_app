namespace veteran_logistic.Reports.QueryBuilder.Models;

/// <summary>
/// Represents filter operators available in the query builder.
/// </summary>
public enum FilterOperator
{
    // Text operators
    /// <summary>
    /// Contains operator for text fields.
    /// </summary>
    Contains,

    /// <summary>
    /// Starts with operator for text fields.
    /// </summary>
    StartsWith,

    /// <summary>
    /// Ends with operator for text fields.
    /// </summary>
    EndsWith,

    /// <summary>
    /// Equals operator for text/number/date fields.
    /// </summary>
    Equals,

    /// <summary>
    /// Not equals operator for text/number/date fields.
    /// </summary>
    NotEquals,

    // Number operators
    /// <summary>
    /// Greater than operator for number fields.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Less than operator for number fields.
    /// </summary>
    LessThan,

    /// <summary>
    /// Greater than or equal operator for number fields.
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Less than or equal operator for number fields.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Between operator for number/date fields.
    /// </summary>
    Between,

    // Date operators
    /// <summary>
    /// Before operator for date fields.
    /// </summary>
    Before,

    /// <summary>
    /// After operator for date fields.
    /// </summary>
    After,

    // Boolean operators
    /// <summary>
    /// Is true operator for boolean fields.
    /// </summary>
    IsTrue,

    /// <summary>
    /// Is false operator for boolean fields.
    /// </summary>
    IsFalse,

    /// <summary>
    /// Is null operator.
    /// </summary>
    IsNull,

    /// <summary>
    /// Is not null operator.
    /// </summary>
    IsNotNull
}
