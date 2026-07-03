using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.Authentication.Contracts;
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
        set => SetProperty(ref _currentSession, value);
    }
}
