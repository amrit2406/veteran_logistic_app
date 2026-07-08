using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Roles.ViewModels;

/// <summary>
/// ViewModel for the Add Role screen.
/// </summary>
public sealed partial class AddRoleViewModel : ViewModelBase
{
    private readonly IRoleCommandService _roleCommandService;
    private readonly INavigationService _navigationService;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddRoleViewModel"/> class.
    /// </summary>
    /// <param name="roleCommandService">The role command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddRoleViewModel(IRoleCommandService roleCommandService, INavigationService navigationService)
    {
        _roleCommandService = roleCommandService ?? throw new ArgumentNullException(nameof(roleCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Role";
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
    /// Command to save the role.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateRoleRequest
        {
            Name = Name,
            Description = Description,
            IsActive = IsActive
        };

        SetBusy("Creating role...");
        var result = await _roleCommandService.CreateRoleAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create role.";
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
