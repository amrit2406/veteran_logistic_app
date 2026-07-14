using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Users.ViewModels;

/// <summary>
/// ViewModel for the Users listing screen.
/// </summary>
public sealed partial class UsersViewModel : ViewModelBase
{
    private readonly IUserQueryService _userQueryService;
    private readonly IUserCommandService _userCommandService;
    private readonly INavigationService _navigationService;
    private readonly IPermissionAuthorizationService _permissionAuthorizationService;
    private string _searchText = string.Empty;
    private UserListItem? _selectedUser;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersViewModel"/> class.
    /// </summary>
    /// <param name="userQueryService">The user query service.</param>
    /// <param name="userCommandService">The user command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="permissionAuthorizationService">The permission authorization service.</param>
    public UsersViewModel(IUserQueryService userQueryService, IUserCommandService userCommandService, INavigationService navigationService, IPermissionAuthorizationService permissionAuthorizationService)
    {
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _permissionAuthorizationService = permissionAuthorizationService ?? throw new ArgumentNullException(nameof(permissionAuthorizationService));
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        GoBackCommand.NotifyCanExecuteChanged();
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadUsersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadUsersAsync(cancellationToken);
        
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher != null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(() =>
            {
                GoBackCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CanGoBack));
                AddUserCommand.NotifyCanExecuteChanged();
            });
        }
        else
        {
            GoBackCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanGoBack));
            AddUserCommand.NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// Gets the collection of users to display.
    /// </summary>
    public ObservableCollection<UserListItem> Users { get; } = new();

    /// <summary>
    /// Gets or sets the search text.
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _ = DebouncedSearchAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected user.
    /// </summary>
    public UserListItem? SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (SetProperty(ref _selectedUser, value))
            {
                EditUserCommand.NotifyCanExecuteChanged();
                ResetPasswordCommand.NotifyCanExecuteChanged();
                ActivateUserCommand.NotifyCanExecuteChanged();
                DeactivateUserCommand.NotifyCanExecuteChanged();
                DeleteUserCommand.NotifyCanExecuteChanged();
            }
        }
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
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Command to refresh the user list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadUsersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add User screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanAddUser))]
    private async Task AddUserAsync()
    {
        await _navigationService.NavigateAsync<AddUserViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit User screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanEditUser))]
    private async Task EditUserAsync()
    {
        if (SelectedUser is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["UserId"] = SelectedUser.Id
        };

        await _navigationService.NavigateAsync<EditUserViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Reset Password screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanResetPassword))]
    private async Task ResetPasswordAsync()
    {
        if (SelectedUser is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["UserId"] = SelectedUser.Id
        };

        await _navigationService.NavigateAsync<ResetPasswordViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected user.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanActivateUser))]
    private async Task ActivateUserAsync()
    {
        if (SelectedUser is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateUserStatusRequest
        {
            UserId = SelectedUser.Id,
            IsActive = true
        };

        SetBusy("Activating user...");
        var result = await _userCommandService.UpdateUserStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleUserStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate user.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected user.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeactivateUser))]
    private async Task DeactivateUserAsync()
    {
        if (SelectedUser is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateUserStatusRequest
        {
            UserId = SelectedUser.Id,
            IsActive = false
        };

        SetBusy("Deactivating user...");
        var result = await _userCommandService.UpdateUserStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleUserStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate user.";
        }
    }

    private async Task HandleUserStatusUpdateSuccess()
    {
        await LoadUsersAsync();
        SelectedUser = null;
        ActivateUserCommand.NotifyCanExecuteChanged();
        DeactivateUserCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected user.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanDeleteUser))]
    private async Task DeleteUserAsync()
    {
        if (SelectedUser is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this user?\n\nThis action hides the user from the application.",
            "Delete User",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteUserRequest
        {
            UserId = SelectedUser.Id
        };

        SetBusy("Deleting user...");
        var result = await _userCommandService.DeleteUserAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadUsersAsync();
            SelectedUser = null;
            DeleteUserCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete user.";
        }
    }

    private bool CanAddUser()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.AddUsers);
    }

    private bool CanEditUser()
    {
        return SelectedUser is not null && _permissionAuthorizationService.HasPermission(ApplicationPermission.EditUsers);
    }

    private bool CanResetPassword()
    {
        return SelectedUser is not null && _permissionAuthorizationService.HasPermission(ApplicationPermission.EditUsers);
    }

    private bool CanActivateUser()
    {
        return SelectedUser is not null && _permissionAuthorizationService.HasPermission(ApplicationPermission.ActivateUsers);
    }

    private bool CanDeactivateUser()
    {
        return SelectedUser is not null && _permissionAuthorizationService.HasPermission(ApplicationPermission.ActivateUsers);
    }

    private bool CanDeleteUser()
    {
        return SelectedUser is not null && _permissionAuthorizationService.HasPermission(ApplicationPermission.DeleteUsers);
    }

    /// <summary>
    /// Loads all users.
    /// </summary>
    private async Task LoadUsersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading users...");
        var users = await _userQueryService.GetAllUsersAsync(cancellationToken);
        UpdateUsers(users);
        ClearBusy();
    }

    /// <summary>
    /// Debounced search to prevent excessive database queries.
    /// </summary>
    private async Task DebouncedSearchAsync()
    {
        // Cancel and dispose previous search if still running
        var cts = _searchCancellationTokenSource;
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }

        _searchCancellationTokenSource = new CancellationTokenSource();
        var token = _searchCancellationTokenSource.Token;

        try
        {
            // Wait 300ms to allow user to finish typing
            await Task.Delay(300, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            await SearchUsersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches users based on the current search text.
    /// </summary>
    private async Task SearchUsersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching users...");
        var users = await _userQueryService.SearchUsersAsync(SearchText, cancellationToken);
        UpdateUsers(users);
        ClearBusy();
    }

    private void UpdateUsers(IEnumerable<UserListItem> users)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateUsersInternal(users);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateUsersInternal(users));
        }
    }

    private void UpdateUsersInternal(IEnumerable<UserListItem> users)
    {
        Users.Clear();
        foreach (var user in users)
        {
            Users.Add(user);
        }
    }
}
