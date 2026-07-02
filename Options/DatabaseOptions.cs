namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Database behavior and provider-specific settings. Bound to the "Database" configuration section.
/// This POCO intentionally does NOT contain the connection string; use IConfiguration.GetConnectionString("DefaultConnection") instead.
/// </summary>
public sealed class DatabaseOptions
{
    /// <summary>
    /// Command timeout in seconds for database commands. If 0 or negative, provider default is used.
    /// </summary>
    public int CommandTimeout { get; set; } = 30;

    /// <summary>
    /// Whether to enable EF Core sensitive data logging (may expose parameters). Use only in development.
    /// </summary>
    public bool EnableSensitiveDataLogging { get; set; } = false;

    /// <summary>
    /// Whether to enable detailed errors from EF Core.
    /// </summary>
    public bool EnableDetailedErrors { get; set; } = false;

    /// <summary>
    /// Whether to enable retry on failure for transient SQL errors.
    /// </summary>
    public bool EnableRetryOnFailure { get; set; } = true;

    /// <summary>
    /// Maximum number of retry attempts when EnableRetryOnFailure is true.
    /// </summary>
    public int MaxRetryCount { get; set; } = 5;

    /// <summary>
    /// Maximum delay between retries in seconds.
    /// </summary>
    public int MaxRetryDelaySeconds { get; set; } = 30;
}
