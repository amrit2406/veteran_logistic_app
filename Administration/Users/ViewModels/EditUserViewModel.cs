using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using veteran_logistic.Authorization.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Users.ViewModels;

/// <summary>
/// ViewModel for the Edit User screen.
/// </summary>
public sealed partial class EditUserViewModel : ViewModelBase, INavigationAware
{
    private readonly IUserCommandService _userCommandService;
    private readonly IUserQueryService _userQueryService;
    private readonly INavigationService _navigationService;
    private int _userId;
    private string _username = string.Empty;
    private string _displayName = string.Empty;
    private string _selectedRole = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditUserViewModel"/> class.
    /// </summary>
    /// <param name="userCommandService">The user command service.</param>
    /// <param name="userQueryService">The user query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditUserViewModel(IUserCommandService userCommandService, IUserQueryService userQueryService, INavigationService navigationService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit User";
        LoadRoles();
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("UserId", out var userId))
        {
            _userId = userId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadUserAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the username (read-only).
    /// </summary>
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    /// <summary>
    /// Gets or sets the selected role.
    /// </summary>
    public string SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value);
    }

    /// <summary>
    /// Gets or sets whether the user is active.
    /// </summary>
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    /// <summary>
    /// Gets the collection of available roles.
    /// </summary>
    public ObservableCollection<string> Roles { get; } = new();

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Loads the user data for editing.
    /// </summary>
    private async Task LoadUserAsync(CancellationToken cancellationToken = default)
    {
        if (_userId == 0)
        {
            ValidationError = "User ID is required.";
            return;
        }

        SetBusy("Loading user...");
        var user = await _userQueryService.GetUserForEditAsync(_userId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (user is null)
        {
            ValidationError = "User not found.";
            return;
        }

        Username = user.Username;
        DisplayName = user.DisplayName;
        SelectedRole = user.Role;
        IsActive = user.IsActive;
    }

    /// <summary>
    /// Command to save the user.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateUserRequest
        {
            UserId = _userId,
            DisplayName = DisplayName,
            Role = SelectedRole,
            IsActive = IsActive
        };

        SetBusy("Updating user...");
        var result = await _userCommandService.UpdateUserAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update user.";
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

    /// <summary>
    /// Loads roles from the existing authorization infrastructure.
    /// </summary>
    private void LoadRoles()
    {
        Roles.Clear();
        
        foreach (var role in Enum.GetValues<ApplicationRole>())
        {
            if (role != ApplicationRole.None)
            {
                Roles.Add(role.ToString());
            }
        }
    }
}
