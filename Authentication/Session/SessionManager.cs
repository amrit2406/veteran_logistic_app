using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Events;
using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Session;

/// <summary>
/// Stores runtime session information only.
/// </summary>
public sealed class SessionManager : ObservableObject, ISessionManager
{
    private UserSession? _currentSession;

    /// <summary>
    /// Gets or sets the current user session.
    /// </summary>
    public UserSession? CurrentSession
    {
        get => _currentSession;
        set
        {
            if (SetProperty(ref _currentSession, value))
            {
                OnSessionChanged();
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether there is an active session.
    /// </summary>
    public bool HasActiveSession => _currentSession != null && _currentSession.IsActive;

    /// <summary>
    /// Gets the session identifier.
    /// </summary>
    public string? SessionId => _currentSession?.SessionId;

    /// <summary>
    /// Gets the time the session started.
    /// </summary>
    public DateTimeOffset LoginTime => _currentSession?.LoginTime ?? DateTimeOffset.MinValue;

    /// <summary>
    /// Gets the last time the session was updated.
    /// </summary>
    public DateTimeOffset LastActivity => _currentSession?.LastActivity ?? DateTimeOffset.MinValue;

    /// <summary>
    /// Occurs when the session changes.
    /// </summary>
    public event EventHandler<SessionChangedEventArgs>? SessionChanged;

    /// <summary>
    /// Determines whether the current session is valid.
    /// </summary>
    /// <returns>True if the session is valid; otherwise, false.</returns>
    public bool IsSessionValid()
    {
        return HasActiveSession && _currentSession != null;
    }

    /// <summary>
    /// Updates the last activity timestamp for the current session.
    /// </summary>
    public void UpdateActivity()
    {
        if (_currentSession != null)
        {
            _currentSession.UpdateLastActivity();
            OnPropertyChanged(nameof(LastActivity));
            OnSessionChanged();
        }
    }

    /// <summary>
    /// Clears the current session.
    /// </summary>
    public void ClearSession()
    {
        CurrentSession = null;
    }

    private void OnSessionChanged()
    {
        var sessionInfo = CreateSessionInfo();
        SessionChanged?.Invoke(this, new SessionChangedEventArgs(sessionInfo));
    }

    private SessionInfo? CreateSessionInfo()
    {
        if (_currentSession == null)
        {
            return null;
        }

        return new SessionInfo
        {
            SessionId = _currentSession.SessionId,
            LoginTime = _currentSession.LoginTime,
            LastActivity = _currentSession.LastActivity,
            AuthenticatedUser = _currentSession.AuthenticatedUser,
            FinancialYear = _currentSession.FinancialYear
        };
    }
}
