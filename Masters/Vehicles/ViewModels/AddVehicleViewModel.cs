using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Vehicles.ViewModels;

/// <summary>
/// ViewModel for the Add Vehicle screen.
/// </summary>
public sealed partial class AddVehicleViewModel : ViewModelBase
{
    private readonly IVehicleCommandService _vehicleCommandService;
    private readonly IVehicleOwnerQueryService _vehicleOwnerQueryService;
    private readonly INavigationService _navigationService;
    private int _vehicleOwnerId;
    private string _vehicleNumber = string.Empty;
    private string _vehicleType = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddVehicleViewModel"/> class.
    /// </summary>
    /// <param name="vehicleCommandService">The vehicle command service.</param>
    /// <param name="vehicleOwnerQueryService">The vehicle owner query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddVehicleViewModel(IVehicleCommandService vehicleCommandService, IVehicleOwnerQueryService vehicleOwnerQueryService, INavigationService navigationService)
    {
        _vehicleCommandService = vehicleCommandService ?? throw new ArgumentNullException(nameof(vehicleCommandService));
        _vehicleOwnerQueryService = vehicleOwnerQueryService ?? throw new ArgumentNullException(nameof(vehicleOwnerQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Vehicle";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadVehicleOwnersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the vehicle type options.
    /// </summary>
    public ObservableCollection<string> VehicleTypes { get; } = new ObservableCollection<string>
    {
        "10 Wheels",
        "12 Wheels",
        "14 Wheels",
        "16 Wheels",
        "18 Wheels",
        "20 Wheels",
        "22 Wheels"
    };

    /// <summary>
    /// Gets the collection of vehicle owners for the dropdown.
    /// </summary>
    public ObservableCollection<VehicleOwnerListItem> VehicleOwners { get; } = new();

    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int VehicleOwnerId
    {
        get => _vehicleOwnerId;
        set => SetProperty(ref _vehicleOwnerId, value);
    }

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string VehicleNumber
    {
        get => _vehicleNumber;
        set => SetProperty(ref _vehicleNumber, value);
    }

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType
    {
        get => _vehicleType;
        set => SetProperty(ref _vehicleType, value);
    }

    /// <summary>
    /// Gets or sets whether the vehicle is active.
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
    /// Command to save the vehicle.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateVehicleRequest
        {
            VehicleOwnerId = VehicleOwnerId,
            VehicleNumber = VehicleNumber,
            VehicleType = VehicleType,
            IsActive = IsActive
        };

        SetBusy("Creating vehicle...");
        var result = await _vehicleCommandService.CreateVehicleAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create vehicle.";
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

    /// <summary>
    /// Loads vehicle owners for the dropdown.
    /// </summary>
    private async Task LoadVehicleOwnersAsync(CancellationToken cancellationToken = default)
    {
        var vehicleOwners = await _vehicleOwnerQueryService.GetAllVehicleOwnersAsync(cancellationToken).ConfigureAwait(false);
        
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher != null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(() =>
            {
                VehicleOwners.Clear();
                foreach (var owner in vehicleOwners)
                {
                    VehicleOwners.Add(owner);
                }
            });
        }
        else
        {
            VehicleOwners.Clear();
            foreach (var owner in vehicleOwners)
            {
                VehicleOwners.Add(owner);
            }
        }
    }
}
