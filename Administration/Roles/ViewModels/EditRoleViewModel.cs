using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Roles.ViewModels;

/// <summary>
/// ViewModel for the Edit Role screen.
/// </summary>
public sealed partial class EditRoleViewModel : ViewModelBase, INavigationAware
{
    private readonly IRoleCommandService _roleCommandService;
    private readonly IRoleQueryService _roleQueryService;
    private readonly INavigationService _navigationService;
    private int _roleId;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditRoleViewModel"/> class.
    /// </summary>
    /// <param name="roleCommandService">The role command service.</param>
    /// <param name="roleQueryService">The role query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditRoleViewModel(IRoleCommandService roleCommandService, IRoleQueryService roleQueryService, INavigationService navigationService)
    {
        _roleCommandService = roleCommandService ?? throw new ArgumentNullException(nameof(roleCommandService));
        _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Role";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("RoleId", out var roleId))
        {
            _roleId = roleId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadRoleAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Gets or sets the role description.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Gets or sets whether the role is active.
    /// </summary>
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
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
    /// Loads the role data for editing.
    /// </summary>
    private async Task LoadRoleAsync(CancellationToken cancellationToken = default)
    {
        if (_roleId == 0)
        {
            ValidationError = "Role ID is required.";
            return;
        }

        SetBusy("Loading role...");
        var role = await _roleQueryService.GetRoleForEditAsync(_roleId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (role is null)
        {
            ValidationError = "Role not found.";
            return;
        }

        Name = role.Name;
        Description = role.Description;
        IsActive = role.IsActive;
    }

    /// <summary>
    /// Command to save the role.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateRoleRequest
        {
            Id = _roleId,
            Name = Name,
            Description = Description,
            IsActive = IsActive
        };

        SetBusy("Updating role...");
        var result = await _roleCommandService.UpdateRoleAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update role.";
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
