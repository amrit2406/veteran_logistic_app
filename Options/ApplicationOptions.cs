using System.Globalization;

namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Strongly-typed application settings.
/// Bound to the "Application" configuration section.
/// </summary>
public sealed class ApplicationOptions
{
    /// <summary>
    /// Application friendly name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Application semantic version.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Company or vendor name.
    /// </summary>
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// Default culture for the application (e.g. en-US).
    /// </summary>
    public string DefaultCulture { get; set; } = CultureInfo.InvariantCulture.Name;

    /// <summary>
    /// Default UI theme identifier.
    /// </summary>
    public string DefaultTheme { get; set; } = "Light";
}
