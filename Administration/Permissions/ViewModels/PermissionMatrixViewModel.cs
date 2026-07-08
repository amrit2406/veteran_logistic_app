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
    private readonly IPermissionCommandService _permissionCommandService;
    private ObservableCollection<PermissionMatrixRow> _permissions = new();
    private ObservableCollection<RoleMatrixItem> _roles = new();
    private RoleMatrixItem? _selectedRole;
    private HashSet<int> _originalGrantedPermissionIds = new();
    private HashSet<int> _currentGrantedPermissionIds = new();
    private bool _hasUnsavedChanges;
    private string? _validationError;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionMatrixViewModel"/> class.
    /// </summary>
    /// <param name="permissionQueryService">The permission query service.</param>
    /// <param name="permissionCommandService">The permission command service.</param>
    public PermissionMatrixViewModel(IPermissionQueryService permissionQueryService, IPermissionCommandService permissionCommandService)
    {
        _permissionQueryService = permissionQueryService ?? throw new ArgumentNullException(nameof(permissionQueryService));
        _permissionCommandService = permissionCommandService ?? throw new ArgumentNullException(nameof(permissionCommandService));
        
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
    /// Gets or sets the currently selected role.
    /// </summary>
    public RoleMatrixItem? SelectedRole
    {
        get => _selectedRole;
        set
        {
            if (SetProperty(ref _selectedRole, value))
            {
                OnSelectedRoleChanged();
            }
        }
    }

    /// <summary>
    /// Gets whether there are unsaved changes.
    /// </summary>
    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        private set => SetProperty(ref _hasUnsavedChanges, value);
    }

    /// <summary>
    /// Gets whether the save command can execute.
    /// </summary>
    // Removed CanSave method; button enable handled via XAML binding

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string? ValidationError
    {
        get => _validationError;
        private set => SetProperty(ref _validationError, value);
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
    /// Command to save permission changes.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedRole is null)
        {
            return;
        }

        SetBusy("Saving permissions...");
        ValidationError = null;

        try
        {
            var request = new AssignPermissionsRequest
            {
                RoleId = SelectedRole.RoleId,
                PermissionAssignments = Permissions
                    .Select(p => new PermissionAssignmentItem
                    {
                        PermissionId = p.PermissionId,
                        IsGranted = _currentGrantedPermissionIds.Contains(p.PermissionId)
                    })
                    .ToList()
            };

            var result = await _permissionCommandService.AssignPermissionsAsync(request).ConfigureAwait(false);

            if (!result.IsSuccess)
            {
                ValidationError = result.ErrorMessage;
                return;
            }

            // Refresh the matrix to reflect saved changes
            await LoadPermissionMatrixAsync().ConfigureAwait(false);

            // Clear dirty state
            _originalGrantedPermissionIds = new HashSet<int>(_currentGrantedPermissionIds);
            HasUnsavedChanges = false;
            SaveCommand.NotifyCanExecuteChanged();
        }
        catch (Exception ex)
        {
            // Surface the exception message for debugging
            ValidationError = $"An unexpected error occurred while saving permissions: {ex.Message}";
        }
        finally
        {
            ClearBusy();
        }
    }

    /// <summary>
    /// Command to cancel unsaved changes and reload the matrix.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await LoadPermissionMatrixAsync().ConfigureAwait(false);
    }

    // Determines if Cancel can execute
    // Removed CanCancel method; button enable handled via XAML binding

    /// <summary>
    /// Handles when a permission checkbox is toggled from the UI.
    /// </summary>
    /// <param name="permissionRow">The permission row that was toggled.</param>
    [RelayCommand]
    private void OnPermissionToggled(PermissionMatrixRow permissionRow)
    {
        if (SelectedRole is null)
        {
            return;
        }

        if (permissionRow.IsGranted)
        {
            _currentGrantedPermissionIds.Add(permissionRow.PermissionId);
        }
        else
        {
            _currentGrantedPermissionIds.Remove(permissionRow.PermissionId);
        }

        // Check if there are changes
        HasUnsavedChanges = !_currentGrantedPermissionIds.SetEquals(_originalGrantedPermissionIds);
        ValidationError = null;
        SaveCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Checks if a permission is granted for the selected role.
    /// </summary>
    /// <param name="permissionId">The permission ID.</param>
    /// <returns>True if the permission is granted, false otherwise.</returns>
    public bool IsPermissionGranted(int permissionId)
    {
        return _currentGrantedPermissionIds.Contains(permissionId);
    }

    /// <summary>
    /// Handles when the selected role changes.
    /// </summary>
    private void OnSelectedRoleChanged()
    {
        if (SelectedRole is null)
        {
            _currentGrantedPermissionIds.Clear();
            _originalGrantedPermissionIds.Clear();
            HasUnsavedChanges = false;
            ValidationError = null;
            
            // Clear all IsGranted properties
            foreach (var permission in Permissions)
            {
                permission.IsGranted = false;
            }
        }
        else
        {
            // Load the current permissions for the selected role
            _originalGrantedPermissionIds = new HashSet<int>(SelectedRole.GrantedPermissionIds);
            _currentGrantedPermissionIds = new HashSet<int>(SelectedRole.GrantedPermissionIds);
            HasUnsavedChanges = false;
            ValidationError = null;
            
            // Update IsGranted properties for all permissions
            foreach (var permission in Permissions)
            {
                permission.IsGranted = _currentGrantedPermissionIds.Contains(permission.PermissionId);
            }
        }

        SaveCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
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
