namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Request model for updating material status.
/// </summary>
public sealed class UpdateMaterialStatusRequest
{
    /// <summary>
    /// Gets or sets the material ID.
    /// </summary>
    public int MaterialId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
