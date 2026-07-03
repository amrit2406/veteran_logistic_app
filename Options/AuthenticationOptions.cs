namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Strongly-typed authentication settings.
/// Bound to the "Authentication" configuration section.
/// </summary>
public sealed class AuthenticationOptions
{
    /// <summary>
    /// Default administrator account settings.
    /// </summary>
    public DefaultAdminOptions DefaultAdmin { get; set; } = new();
}

/// <summary>
/// Default administrator account configuration.
/// </summary>
public sealed class DefaultAdminOptions
{
    /// <summary>
    /// Default administrator username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Default administrator password.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Default administrator display name.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Default administrator email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
