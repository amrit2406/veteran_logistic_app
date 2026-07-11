namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Data transfer object for displaying material information in a list.
/// </summary>
public sealed class MaterialListItem
{
    /// <summary>
    /// Gets or sets the material ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the material is active.
    /// </summary>
    public bool IsActive { get; set; }
}
