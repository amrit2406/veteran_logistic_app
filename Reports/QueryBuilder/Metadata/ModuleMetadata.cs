namespace veteran_logistic.Reports.QueryBuilder.Metadata;

/// <summary>
/// Represents metadata for a module in the query builder.
/// </summary>
public sealed class ModuleMetadata
{
    /// <summary>
    /// Gets or sets the unique identifier for the module.
    /// </summary>
    public string ModuleId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for the module.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of fields available in this module.
    /// </summary>
    public List<FieldMetadata> Fields { get; set; } = new();
}
