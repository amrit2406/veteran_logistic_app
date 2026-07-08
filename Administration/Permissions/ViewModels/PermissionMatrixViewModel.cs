using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Administration.Permissions.Contracts;
using veteran_logistic.Administration.Permissions.Models;
using veteran_logistic.MVVM;

namespace veteran_logistic.Administration.Permissions.ViewModels;

/// <summary>
/// ViewModel for the Permission Matrix screen.
/// </summary>
public sealed partial class PermissionMatrixViewModel : ViewModelBase
{
    private readonly IPermissionQueryService _permissionQueryService;
    private ObservableCollection<PermissionMatrixRow> _permissions = new();
    private ObservableCollection<RoleMatrixItem> _roles = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionMatrixViewModel"/> class.
    /// </summary>
    /// <param name="permissionQueryService">The permission query service.</param>
    public PermissionMatrixViewModel(IPermissionQueryService permissionQueryService)
    {
        _permissionQueryService = permissionQueryService ?? throw new ArgumentNullException(nameof(permissionQueryService));
        
        Title = "Permission Matrix";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadPermissionMatrixAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadPermissionMatrixAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of roles to display as columns.
    /// </summary>
    public ObservableCollection<RoleMatrixItem> Roles
    {
        get => _roles;
        private set => SetProperty(ref _roles, value);
    }

    /// <summary>
    /// Gets the collection of permissions to display as rows.
    /// </summary>
    public ObservableCollection<PermissionMatrixRow> Permissions
    {
        get => _permissions;
        private set => SetProperty(ref _permissions, value);
    }

    /// <summary>
    /// Command to refresh the permission matrix.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadPermissionMatrixAsync();
    }

    /// <summary>
    /// Loads the permission matrix data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPermissionMatrixAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading permission matrix...");
        var matrix = await _permissionQueryService.GetPermissionMatrixAsync(cancellationToken).ConfigureAwait(false);
        
        UpdateRoles(matrix.Roles);
        UpdatePermissions(matrix.Permissions);
        
        ClearBusy();
    }

    /// <summary>
    /// Updates the roles collection on the UI thread.
    /// </summary>
    /// <param name="roles">The roles to update.</param>
    private void UpdateRoles(IEnumerable<RoleMatrixItem> roles)
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
    private void UpdateRolesInternal(IEnumerable<RoleMatrixItem> roles)
    {
        Roles.Clear();
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }

    /// <summary>
    /// Updates the permissions collection on the UI thread.
    /// </summary>
    /// <param name="permissions">The permissions to update.</param>
    private void UpdatePermissions(IEnumerable<PermissionMatrixRow> permissions)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdatePermissionsInternal(permissions);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdatePermissionsInternal(permissions));
        }
    }

    /// <summary>
    /// Updates the permissions collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="permissions">The permissions to update.</param>
    private void UpdatePermissionsInternal(IEnumerable<PermissionMatrixRow> permissions)
    {
        Permissions.Clear();
        foreach (var permission in permissions)
        {
            Permissions.Add(permission);
        }
    }
}
