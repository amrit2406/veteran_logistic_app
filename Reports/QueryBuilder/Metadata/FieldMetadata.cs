namespace veteran_logistic.Reports.QueryBuilder.Metadata;

/// <summary>
/// Represents metadata for a field available in the query builder.
/// </summary>
public sealed class FieldMetadata
{
    /// <summary>
    /// Gets or sets the unique identifier for the field.
    /// </summary>
    public string FieldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for the field.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data type of the field.
    /// </summary>
    public FieldDataType DataType { get; set; }

    /// <summary>
    /// Gets or sets the property path in the entity (e.g., "ChallanNumber", "Vehicle.VehicleNumber").
    /// </summary>
    public string PropertyPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the field can be used for grouping.
    /// </summary>
    public bool CanGroup { get; set; }

    /// <summary>
    /// Gets or sets whether the field can be aggregated.
    /// </summary>
    public bool CanAggregate { get; set; }

    /// <summary>
    /// Gets or sets the default width for the column in the results grid.
    /// </summary>
    public double DefaultWidth { get; set; } = 100;
}
