using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.MVVM;

namespace veteran_logistic.Authentication.ViewModels;

/// <summary>
/// ViewModel for the Login screen.
/// This is UI-only - no authentication logic is implemented in this phase.
/// </summary>
public sealed partial class LoginViewModel : ViewModelBase
{
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;
    private bool _isPasswordVisible;

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
    /// Placeholder command for login action.
    /// Authentication logic will be implemented in Phase 1.4.
    /// </summary>
    [RelayCommand]
    private void Login()
    {
        // TODO: Implement authentication workflow in Phase 1.4
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    [RelayCommand]
    private void Exit()
    {
        // Placeholder - will be connected to application shutdown in a later phase
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
