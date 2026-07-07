using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Users.ViewModels;

/// <summary>
/// ViewModel for the Reset Password screen.
/// </summary>
public sealed partial class ResetPasswordViewModel : ViewModelBase, INavigationAware
{
    private readonly IUserCommandService _userCommandService;
    private readonly INavigationService _navigationService;
    private int _userId;
    private string _newPassword = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResetPasswordViewModel"/> class.
    /// </summary>
    /// <param name="userCommandService">The user command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public ResetPasswordViewModel(IUserCommandService userCommandService, INavigationService navigationService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Reset Password";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("UserId", out var userId))
        {
            _userId = userId;
        }
    }

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string NewPassword
    {
        get => _newPassword;
        set => SetProperty(ref _newPassword, value);
    }

    /// <summary>
    /// Gets or sets the password confirmation.
    /// </summary>
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Command to save the new password.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new ResetPasswordRequest
        {
            UserId = _userId,
            NewPassword = NewPassword,
            ConfirmPassword = ConfirmPassword
        };

        SetBusy("Resetting password...");
        var result = await _userCommandService.ResetPasswordAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to reset password.";
        }
    }

    /// <summary>
    /// Command to cancel and navigate back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync().ConfigureAwait(false);
    }
}
