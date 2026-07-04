namespace veteran_logistic.Authentication.Events;

/// <summary>
/// Provides data for the SessionChanged event.
/// </summary>
public sealed class SessionChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionChangedEventArgs"/> class.
    /// </summary>
    /// <param name="sessionInfo">The session information, or null if the session was cleared.</param>
    public SessionChangedEventArgs(Models.SessionInfo? sessionInfo)
    {
        SessionInfo = sessionInfo;
    }

    /// <summary>
    /// Gets the session information, or null if the session was cleared.
    /// </summary>
    public Models.SessionInfo? SessionInfo { get; }
}
