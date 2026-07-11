namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Request model for updating an existing material.
/// </summary>
public sealed class UpdateMaterialRequest
{
    /// <summary>
    /// Gets or sets the material ID.
    /// </summary>
    public int MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the material is active.
    /// </summary>
    public bool IsActive { get; set; }
}
