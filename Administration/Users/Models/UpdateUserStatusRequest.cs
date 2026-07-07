namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Request model for updating a user's active status.
/// </summary>
public sealed class UpdateUserStatusRequest
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
