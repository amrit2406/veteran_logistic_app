using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using veteran_logistic.Authorization.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Users.ViewModels;

/// <summary>
/// ViewModel for the Add User screen.
/// </summary>
public sealed partial class AddUserViewModel : ViewModelBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly IRoleQueryService _roleQueryService;
    private readonly INavigationService _navigationService;
    private string _username = string.Empty;
    private string _displayName = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _selectedRole = string.Empty;
    private RoleListItem? _selectedRoleItem;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUserViewModel"/> class.
    /// </summary>
    /// <param name="userCommandService">The user command service.</param>
    /// <param name="roleQueryService">The role query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddUserViewModel(IUserCommandService userCommandService, IRoleQueryService roleQueryService, INavigationService navigationService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add User";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadRolesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the username.
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
    /// Gets or sets the password.
    /// </summary>
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    /// <summary>
    /// Gets or sets the confirm password.
    /// </summary>
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    /// <summary>
    /// Gets or sets the selected role name (for API).
    /// </summary>
    public string SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value);
    }

    /// <summary>
    /// Gets or sets the selected role item (for UI binding).
    /// </summary>
    public RoleListItem? SelectedRoleItem
    {
        get => _selectedRoleItem;
        set
        {
            if (SetProperty(ref _selectedRoleItem, value))
            {
                SelectedRole = value?.Name ?? string.Empty;
            }
        }
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
    public ObservableCollection<RoleListItem> Roles { get; } = new();

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Command to save the user.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateUserRequest
        {
            Username = Username,
            DisplayName = DisplayName,
            Password = Password,
            ConfirmPassword = ConfirmPassword,
            Role = SelectedRole,
            IsActive = IsActive
        };

        SetBusy("Creating user...");
        var result = await _userCommandService.CreateUserAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create user.";
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
    /// Loads roles from the database.
    /// </summary>
    private async Task LoadRolesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading roles...");
        var roles = await _roleQueryService.GetAllRolesAsync(cancellationToken).ConfigureAwait(false);
        ClearBusy();

        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher != null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(() =>
            {
                Roles.Clear();
                foreach (var role in roles)
                {
                    if (role.IsActive)
                    {
                        Roles.Add(role);
                    }
                }
            });
        }
        else
        {
            Roles.Clear();
            foreach (var role in roles)
            {
                if (role.IsActive)
                {
                    Roles.Add(role);
                }
            }
        }
    }
}
