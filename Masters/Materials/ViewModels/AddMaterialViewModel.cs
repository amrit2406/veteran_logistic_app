using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Materials.ViewModels;

/// <summary>
/// ViewModel for the Add Material screen.
/// </summary>
public sealed partial class AddMaterialViewModel : ViewModelBase
{
    private readonly IMaterialCommandService _materialCommandService;
    private readonly INavigationService _navigationService;
    private string _materialName = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddMaterialViewModel"/> class.
    /// </summary>
    /// <param name="materialCommandService">The material command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddMaterialViewModel(IMaterialCommandService materialCommandService, INavigationService navigationService)
    {
        _materialCommandService = materialCommandService ?? throw new ArgumentNullException(nameof(materialCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Material";
    }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string MaterialName
    {
        get => _materialName;
        set => SetProperty(ref _materialName, value);
    }

    /// <summary>
    /// Gets or sets whether the material is active.
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
    /// Command to save the material.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateMaterialRequest
        {
            MaterialName = MaterialName,
            IsActive = IsActive
        };

        SetBusy("Creating material...");
        var result = await _materialCommandService.CreateMaterialAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create material.";
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
