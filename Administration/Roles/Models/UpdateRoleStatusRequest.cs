namespace veteran_logistic.Administration.Roles.Models;

/// <summary>
/// Request model for updating a role's active status.
/// </summary>
public sealed class UpdateRoleStatusRequest
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
