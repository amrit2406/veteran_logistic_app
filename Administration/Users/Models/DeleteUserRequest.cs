namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Request model for deleting a user.
/// </summary>
public sealed class DeleteUserRequest
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }
}
