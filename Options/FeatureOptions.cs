namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Feature flags for toggling optional functionality. Bound to the "Features" section.
/// </summary>
public sealed class FeatureOptions
{
    /// <summary>
    /// Enable dashboard UI and related services.
    /// </summary>
    public bool EnableDashboard { get; set; } = true;

    /// <summary>
    /// Enable reporting features.
    /// </summary>
    public bool EnableReporting { get; set; } = true;

    /// <summary>
    /// Enable user notifications within the UI.
    /// </summary>
    public bool EnableNotifications { get; set; } = true;

    /// <summary>
    /// Enable audit trail capture.
    /// </summary>
    public bool EnableAuditTrail { get; set; } = false;
}
