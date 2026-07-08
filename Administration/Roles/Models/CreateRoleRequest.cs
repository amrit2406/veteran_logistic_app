namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Request model for creating a new role.
/// </summary>
public sealed class CreateRoleRequest
{
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
    public bool IsActive { get; set; } = true;
}
