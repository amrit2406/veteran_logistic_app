using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Authentication.ViewModels;

/// <summary>
/// ViewModel for the Login screen.
/// </summary>
public sealed partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly INavigationService _navigationService;
    private readonly IRememberMeService _rememberMeService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;
    private bool _isPasswordVisible;
    private bool _rememberMe;

    /// <summary>
    /// Initializes a new instance of the LoginViewModel.
    /// </summary>
    /// <param name="authenticationService">The authentication service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="rememberMeService">The Remember Me service.</param>
    public LoginViewModel(
        IAuthenticationService authenticationService,
        INavigationService navigationService,
        IRememberMeService rememberMeService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _rememberMeService = rememberMeService ?? throw new ArgumentNullException(nameof(rememberMeService));
    }

    /// <inheritdoc />
    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await RestoreRememberMeSettingsAsync(cancellationToken).ConfigureAwait(false);
        await base.InitializeAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets or sets the username entered by the user.
    /// </summary>
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    /// <summary>
    /// Gets or sets the password entered by the user.
    /// </summary>
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    /// <summary>
    /// Gets or sets the error message to display.
    /// </summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <summary>
    /// Gets or sets whether the password is visible.
    /// </summary>
    public bool IsPasswordVisible
    {
        get => _isPasswordVisible;
        set => SetProperty(ref _isPasswordVisible, value);
    }

    /// <summary>
    /// Gets or sets whether Remember Me is enabled.
    /// </summary>
    public bool RememberMe
    {
        get => _rememberMe;
        set => SetProperty(ref _rememberMe, value);
    }

    /// <summary>
    /// Command to authenticate the user and navigate to the Shell.
    /// </summary>
    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            SetBusy("Authenticating...");
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter a username.";
                ClearBusy();
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter a password.";
                ClearBusy();
                return;
            }

            var request = new LoginRequest
            {
                Username = Username,
                Password = Password
            };

            var result = await _authenticationService.AuthenticateAsync(request);

            if (result.IsSuccess)
            {
                await PersistRememberMeSettingsAsync();

                // Navigate to the Shell on successful authentication
                await _navigationService.NavigateAsync<Shell.ShellViewModel>();
            }
            else
            {
                // Display error message on failure
                ErrorMessage = result.ErrorMessage ?? "Authentication failed.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            ClearBusy();
        }
    }

    private async Task RestoreRememberMeSettingsAsync(CancellationToken cancellationToken)
    {
        var settings = await _rememberMeService.LoadAsync(cancellationToken).ConfigureAwait(false);

        if (!settings.RememberMe)
        {
            return;
        }

        Username = settings.Username;
        RememberMe = true;
    }

    private async Task PersistRememberMeSettingsAsync()
    {
        if (RememberMe)
        {
            await _rememberMeService.SaveAsync(new RememberMeSettings
            {
                Username = Username,
                RememberMe = true
            }).ConfigureAwait(false);
        }
        else
        {
            await _rememberMeService.ClearAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    [RelayCommand]
    private void Exit()
    {
        System.Windows.Application.Current.Shutdown();
    }

    /// <summary>
    /// Command to toggle password visibility.
    /// </summary>
    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }
}
