using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Materials.ViewModels;

/// <summary>
/// ViewModel for the Edit Material screen.
/// </summary>
public sealed partial class EditMaterialViewModel : ViewModelBase, INavigationAware
{
    private readonly IMaterialCommandService _materialCommandService;
    private readonly IMaterialQueryService _materialQueryService;
    private readonly INavigationService _navigationService;
    private int _materialId;
    private string _materialName = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditMaterialViewModel"/> class.
    /// </summary>
    /// <param name="materialCommandService">The material command service.</param>
    /// <param name="materialQueryService">The material query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditMaterialViewModel(IMaterialCommandService materialCommandService, IMaterialQueryService materialQueryService, INavigationService navigationService)
    {
        _materialCommandService = materialCommandService ?? throw new ArgumentNullException(nameof(materialCommandService));
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Material";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("MaterialId", out var materialId))
        {
            _materialId = materialId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadMaterialAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
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
    /// Loads the material data for editing.
    /// </summary>
    private async Task LoadMaterialAsync(CancellationToken cancellationToken = default)
    {
        if (_materialId == 0)
        {
            ValidationError = "Material ID is required.";
            return;
        }

        SetBusy("Loading material...");
        var material = await _materialQueryService.GetMaterialForEditAsync(_materialId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (material is null)
        {
            ValidationError = "Material not found.";
            return;
        }

        MaterialName = material.MaterialName;
        IsActive = material.IsActive;
    }

    /// <summary>
    /// Command to save the material.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateMaterialRequest
        {
            MaterialId = _materialId,
            MaterialName = MaterialName,
            IsActive = IsActive
        };

        SetBusy("Updating material...");
        var result = await _materialCommandService.UpdateMaterialAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update material.";
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
