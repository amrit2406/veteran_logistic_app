namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Persisted Remember Me preferences. Contains only non-sensitive login preference data.
/// </summary>
public sealed class RememberMeSettings
{
    /// <summary>
    /// Gets or sets the remembered username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether Remember Me is enabled.
    /// </summary>
    public bool RememberMe { get; set; }
}
