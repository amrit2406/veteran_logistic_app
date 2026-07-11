using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.FuelPumps.ViewModels;

/// <summary>
/// ViewModel for the Edit Fuel Pump screen.
/// </summary>
public sealed partial class EditFuelPumpViewModel : ViewModelBase, INavigationAware
{
    private readonly IFuelPumpCommandService _fuelPumpCommandService;
    private readonly IFuelPumpQueryService _fuelPumpQueryService;
    private readonly INavigationService _navigationService;
    private int _fuelPumpId;
    private string _fuelPumpName = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditFuelPumpViewModel"/> class.
    /// </summary>
    /// <param name="fuelPumpCommandService">The fuel pump command service.</param>
    /// <param name="fuelPumpQueryService">The fuel pump query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditFuelPumpViewModel(IFuelPumpCommandService fuelPumpCommandService, IFuelPumpQueryService fuelPumpQueryService, INavigationService navigationService)
    {
        _fuelPumpCommandService = fuelPumpCommandService ?? throw new ArgumentNullException(nameof(fuelPumpCommandService));
        _fuelPumpQueryService = fuelPumpQueryService ?? throw new ArgumentNullException(nameof(fuelPumpQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Fuel Pump";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("FuelPumpId", out var fuelPumpId))
        {
            _fuelPumpId = fuelPumpId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadFuelPumpAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName
    {
        get => _fuelPumpName;
        set => SetProperty(ref _fuelPumpName, value);
    }

    /// <summary>
    /// Gets or sets whether the fuel pump is active.
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
    /// Loads the fuel pump data for editing.
    /// </summary>
    private async Task LoadFuelPumpAsync(CancellationToken cancellationToken = default)
    {
        if (_fuelPumpId == 0)
        {
            ValidationError = "Fuel pump ID is required.";
            return;
        }

        SetBusy("Loading fuel pump...");
        var fuelPump = await _fuelPumpQueryService.GetFuelPumpForEditAsync(_fuelPumpId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (fuelPump is null)
        {
            ValidationError = "Fuel pump not found.";
            return;
        }

        FuelPumpName = fuelPump.FuelPumpName;
        IsActive = fuelPump.IsActive;
    }

    /// <summary>
    /// Command to save the fuel pump.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateFuelPumpRequest
        {
            FuelPumpId = _fuelPumpId,
            FuelPumpName = FuelPumpName,
            IsActive = IsActive
        };

        SetBusy("Updating fuel pump...");
        var result = await _fuelPumpCommandService.UpdateFuelPumpAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update fuel pump.";
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
