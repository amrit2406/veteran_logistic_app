namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Model representing role data for editing.
/// </summary>
public sealed class EditRoleModel
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the role is active.
    /// </summary>
    public bool IsActive { get; set; }
}
