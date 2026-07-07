namespace veteran_logistic.Administration.Users.Models;

/// <summary>
/// Request model for resetting a user's password.
/// </summary>
public sealed class ResetPasswordRequest
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password confirmation.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
