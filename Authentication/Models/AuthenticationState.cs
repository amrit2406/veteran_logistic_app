using CommunityToolkit.Mvvm.ComponentModel;

namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Represents runtime authentication status for the application.
/// </summary>
public sealed class AuthenticationState : ObservableObject
{
    private bool _isAuthenticated;
    private DateTimeOffset? _authenticatedAt;

    /// <summary>
    /// Gets or sets a value indicating whether the application is authenticated.
    /// </summary>
    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        set => SetProperty(ref _isAuthenticated, value);
    }

    /// <summary>
    /// Gets or sets the timestamp when authentication was established.
    /// </summary>
    public DateTimeOffset? AuthenticatedAt
    {
        get => _authenticatedAt;
        set => SetProperty(ref _authenticatedAt, value);
    }
}
