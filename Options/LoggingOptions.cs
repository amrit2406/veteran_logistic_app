namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Strongly-typed logging configuration bound to the "Logging" section.
/// </summary>
public sealed class LoggingOptions
{
    /// <summary>
    /// Minimum log level (e.g. Trace, Debug, Information, Warning, Error, Critical).
    /// This value is configuration-friendly and not enforced by this POCO directly.
    /// </summary>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    /// Enable console logging provider.
    /// </summary>
    public bool EnableConsoleLogging { get; set; } = true;

    /// <summary>
    /// Enable debug logging provider.
    /// </summary>
    public bool EnableDebugLogging { get; set; } = true;

    /// <summary>
    /// Enable file logging. Implementation will be added in later phases.
    /// </summary>
    public bool EnableFileLogging { get; set; } = false;
}
