using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Roles.ViewModels;

/// <summary>
/// ViewModel for the Roles listing screen.
/// </summary>
public sealed partial class RolesViewModel : ViewModelBase
{
    private readonly IRoleQueryService _roleQueryService;
    private readonly IRoleCommandService _roleCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private RoleListItem? _selectedRole;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesViewModel"/> class.
    /// </summary>
    /// <param name="roleQueryService">The role query service.</param>
    /// <param name="roleCommandService">The role command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public RolesViewModel(IRoleQueryService roleQueryService, IRoleCommandService roleCommandService, INavigationService navigationService)
    {
        _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        _roleCommandService = roleCommandService ?? throw new ArgumentNullException(nameof(roleCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
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

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadRolesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of roles to display.
    /// </summary>
    public ObservableCollection<RoleListItem> Roles { get; } = new();

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
    /// Gets or sets the selected role.
    /// </summary>
    public RoleListItem? SelectedRole
    {
        get => _selectedRole;
        set
        {
            if (SetProperty(ref _selectedRole, value))
            {
                ActivateRoleCommand.NotifyCanExecuteChanged();
                DeactivateRoleCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the role list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadRolesAsync();
    }

    /// <summary>
    /// Command to add a new role.
    /// </summary>
    [RelayCommand]
    private async Task AddRoleAsync()
    {
        await _navigationService.NavigateAsync<ViewModels.AddRoleViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Role screen.
    /// </summary>
    [RelayCommand]
    private async Task EditRoleAsync()
    {
        if (SelectedRole is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["RoleId"] = SelectedRole.Id
        };

        await _navigationService.NavigateAsync<ViewModels.EditRoleViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected role.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteRoleCommand))]
    private async Task ActivateRoleAsync()
    {
        if (SelectedRole is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to activate this role?",
            "Activate Role",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new UpdateRoleStatusRequest
        {
            RoleId = SelectedRole.Id,
            IsActive = true
        };

        SetBusy("Activating role...");
        var result = await _roleCommandService.ActivateRoleAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleRoleStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate role.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected role.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteRoleCommand))]
    private async Task DeactivateRoleAsync()
    {
        if (SelectedRole is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to deactivate this role?",
            "Deactivate Role",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new UpdateRoleStatusRequest
        {
            RoleId = SelectedRole.Id,
            IsActive = false
        };

        SetBusy("Deactivating role...");
        var result = await _roleCommandService.DeactivateRoleAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleRoleStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate role.";
        }
    }

    private async Task HandleRoleStatusUpdateSuccess()
    {
        await LoadRolesAsync();
        SelectedRole = null;
        ActivateRoleCommand.NotifyCanExecuteChanged();
        DeactivateRoleCommand.NotifyCanExecuteChanged();
    }

    private bool CanExecuteRoleCommand()
    {
        return SelectedRole is not null;
    }

    /// <summary>
    /// Loads all roles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadRolesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading roles...");
        var roles = await _roleQueryService.GetAllRolesAsync(cancellationToken);
        UpdateRoles(roles);
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
            await SearchRolesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches roles based on the current search text.
    /// </summary>
    private async Task SearchRolesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching roles...");
        var roles = await _roleQueryService.SearchRolesAsync(SearchText, cancellationToken);
        UpdateRoles(roles);
        ClearBusy();
    }

    /// <summary>
    /// Updates the roles collection on the UI thread.
    /// </summary>
    /// <param name="roles">The roles to update.</param>
    private void UpdateRoles(IEnumerable<RoleListItem> roles)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateRolesInternal(roles);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateRolesInternal(roles));
        }
    }

    /// <summary>
    /// Updates the roles collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="roles">The roles to update.</param>
    private void UpdateRolesInternal(IEnumerable<RoleListItem> roles)
    {
        Roles.Clear();
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }
}
