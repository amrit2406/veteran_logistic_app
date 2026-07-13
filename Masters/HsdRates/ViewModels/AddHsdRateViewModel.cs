using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.HsdRates.ViewModels;

/// <summary>
/// ViewModel for the Add HSD Rate screen.
/// </summary>
public sealed partial class AddHsdRateViewModel : ViewModelBase
{
    private readonly IHsdRateCommandService _hsdRateCommandService;
    private readonly IHsdRateQueryService _hsdRateQueryService;
    private readonly INavigationService _navigationService;
    private int _fuelPumpId;
    private DateTime _applicableDate = DateTime.Today;
    private decimal _ratePerLitre;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddHsdRateViewModel"/> class.
    /// </summary>
    /// <param name="hsdRateCommandService">The HSD rate command service.</param>
    /// <param name="hsdRateQueryService">The HSD rate query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddHsdRateViewModel(IHsdRateCommandService hsdRateCommandService, IHsdRateQueryService hsdRateQueryService, INavigationService navigationService)
    {
        _hsdRateCommandService = hsdRateCommandService ?? throw new ArgumentNullException(nameof(hsdRateCommandService));
        _hsdRateQueryService = hsdRateQueryService ?? throw new ArgumentNullException(nameof(hsdRateQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add HSD Rate";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadFuelPumpsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of fuel pumps for dropdown.
    /// </summary>
    public ObservableCollection<FuelPumpDropdownItem> FuelPumps { get; } = new();

    /// <summary>
    /// Gets or sets the selected fuel pump ID.
    /// </summary>
    public int FuelPumpId
    {
        get => _fuelPumpId;
        set => SetProperty(ref _fuelPumpId, value);
    }

    /// <summary>
    /// Gets or sets the applicable date.
    /// </summary>
    public DateTime ApplicableDate
    {
        get => _applicableDate;
        set => SetProperty(ref _applicableDate, value);
    }

    /// <summary>
    /// Gets or sets the rate per litre.
    /// </summary>
    public decimal RatePerLitre
    {
        get => _ratePerLitre;
        set => SetProperty(ref _ratePerLitre, value);
    }

    /// <summary>
    /// Gets or sets whether the HSD rate is active.
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
    /// Loads fuel pumps for dropdown.
    /// </summary>
    private async Task LoadFuelPumpsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading fuel pumps...");
        var fuelPumps = await _hsdRateQueryService.GetFuelPumpsForDropdownAsync(cancellationToken).ConfigureAwait(false);
        UpdateFuelPumps(fuelPumps);
        ClearBusy();
    }

    /// <summary>
    /// Updates the fuel pumps collection on the UI thread.
    /// </summary>
    /// <param name="fuelPumps">The fuel pumps to update.</param>
    private void UpdateFuelPumps(IReadOnlyList<FuelPumpDropdownItem> fuelPumps)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateFuelPumpsInternal(fuelPumps);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateFuelPumpsInternal(fuelPumps));
        }
    }

    /// <summary>
    /// Updates the fuel pumps collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="fuelPumps">The fuel pumps to update.</param>
    private void UpdateFuelPumpsInternal(IReadOnlyList<FuelPumpDropdownItem> fuelPumps)
    {
        FuelPumps.Clear();
        foreach (var fuelPump in fuelPumps)
        {
            FuelPumps.Add(fuelPump);
        }
    }

    /// <summary>
    /// Command to save the HSD rate.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateHsdRateRequest
        {
            FuelPumpId = FuelPumpId,
            ApplicableDate = ApplicableDate,
            RatePerLitre = RatePerLitre,
            IsActive = IsActive
        };

        SetBusy("Creating HSD rate...");
        var result = await _hsdRateCommandService.CreateHsdRateAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create HSD rate.";
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
