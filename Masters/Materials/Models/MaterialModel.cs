namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Data transfer object for detailed material information, used for editing.
/// </summary>
public sealed class MaterialModel
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
