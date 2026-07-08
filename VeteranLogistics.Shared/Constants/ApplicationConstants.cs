namespace VeteranLogistics.Shared.Constants;

/// <summary>
/// Application-level constants used across the solution.
/// </summary>
public static class ApplicationConstants
{
    public const string ApplicationName = "Veteran Logistics";
    public const string ApplicationVersionKey = "Application:Version";
    public const string DefaultCulture = "en-US";
    
    /// <summary>
    /// Debounce delay in milliseconds for search operations to prevent excessive database queries.
    /// </summary>
    public const int SearchDebounceDelayMilliseconds = 300;
}
