namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Request model for creating a new material.
/// </summary>
public sealed class CreateMaterialRequest
{
    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string MaterialName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the material is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
