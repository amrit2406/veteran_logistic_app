using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.MVVM;

namespace veteran_logistic.Administration.Roles.ViewModels;

/// <summary>
/// ViewModel for the Roles listing screen.
/// </summary>
public sealed partial class RolesViewModel : ViewModelBase
{
    private readonly IRoleQueryService _roleQueryService;
    private RoleListItem? _selectedRole;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesViewModel"/> class.
    /// </summary>
    /// <param name="roleQueryService">The role query service.</param>
    public RolesViewModel(IRoleQueryService roleQueryService)
    {
        _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
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
    /// Gets or sets the selected role.
    /// </summary>
    public RoleListItem? SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value);
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
    /// Loads all roles.
    /// </summary>
    private async Task LoadRolesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading roles...");
        var roles = await _roleQueryService.GetAllRolesAsync(cancellationToken);
        UpdateRoles(roles);
        ClearBusy();
    }

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

    private void UpdateRolesInternal(IEnumerable<RoleListItem> roles)
    {
        Roles.Clear();
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }
}
