using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.FuelPumps.ViewModels;

/// <summary>
/// ViewModel for the Add Fuel Pump screen.
/// </summary>
public sealed partial class AddFuelPumpViewModel : ViewModelBase
{
    private readonly IFuelPumpCommandService _fuelPumpCommandService;
    private readonly INavigationService _navigationService;
    private string _fuelPumpName = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddFuelPumpViewModel"/> class.
    /// </summary>
    /// <param name="fuelPumpCommandService">The fuel pump command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddFuelPumpViewModel(IFuelPumpCommandService fuelPumpCommandService, INavigationService navigationService)
    {
        _fuelPumpCommandService = fuelPumpCommandService ?? throw new ArgumentNullException(nameof(fuelPumpCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Fuel Pump";
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
    /// Command to save the fuel pump.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateFuelPumpRequest
        {
            FuelPumpName = FuelPumpName,
            IsActive = IsActive
        };

        SetBusy("Creating fuel pump...");
        var result = await _fuelPumpCommandService.CreateFuelPumpAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create fuel pump.";
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
