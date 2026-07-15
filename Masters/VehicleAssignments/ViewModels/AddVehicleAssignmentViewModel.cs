using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleAssignments.ViewModels;

/// <summary>
/// ViewModel for the Add Vehicle Assignment screen.
/// </summary>
public sealed partial class AddVehicleAssignmentViewModel : ViewModelBase
{
    private readonly IVehicleAssignmentCommandService _vehicleAssignmentCommandService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly INavigationService _navigationService;
    private int _selectedVehicleId;
    private DateTime _assignDate = DateTime.UtcNow;
    private DateTime? _releaseDate;
    private string _validationError = string.Empty;
    private string _vehicleNumber = string.Empty;
    private string _vehicleType = string.Empty;
    private string _ownerPanType = string.Empty;
    private string _ownerPanNumber = string.Empty;
    private string _ownerFirstName = string.Empty;
    private string _ownerMiddleName = string.Empty;
    private string _ownerLastName = string.Empty;
    private string _ownerAddress = string.Empty;
    private string _ownerMobileNumber = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddVehicleAssignmentViewModel"/> class.
    /// </summary>
    /// <param name="vehicleAssignmentCommandService">The vehicle assignment command service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddVehicleAssignmentViewModel(
        IVehicleAssignmentCommandService vehicleAssignmentCommandService,
        IVehicleQueryService vehicleQueryService,
        INavigationService navigationService)
    {
        _vehicleAssignmentCommandService = vehicleAssignmentCommandService ?? throw new ArgumentNullException(nameof(vehicleAssignmentCommandService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Vehicle Assignment";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadVehiclesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of vehicles for selection.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets or sets the selected vehicle ID.
    /// </summary>
    public int SelectedVehicleId
    {
        get => _selectedVehicleId;
        set
        {
            if (SetProperty(ref _selectedVehicleId, value))
            {
                _ = LoadVehicleDetailsAsync();
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
    }


    /// <summary>
    /// Gets or sets the assign date.
    /// </summary>
    public DateTime AssignDate
    {
        get => _assignDate;
        set => SetProperty(ref _assignDate, value);
    }

    /// <summary>
    /// Gets or sets the release date (optional).
    /// </summary>
    public DateTime? ReleaseDate
    {
        get => _releaseDate;
        set => SetProperty(ref _releaseDate, value);
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
    /// Gets or sets the vehicle number (read-only, populated from selected vehicle).
    /// </summary>
    public string VehicleNumber
    {
        get => _vehicleNumber;
        set => SetProperty(ref _vehicleNumber, value);
    }

    /// <summary>
    /// Gets or sets the vehicle type (read-only, populated from selected vehicle).
    /// </summary>
    public string VehicleType
    {
        get => _vehicleType;
        set => SetProperty(ref _vehicleType, value);
    }

    /// <summary>
    /// Gets or sets the owner PAN type.
    /// </summary>
    public string OwnerPanType
    {
        get => _ownerPanType;
        set => SetProperty(ref _ownerPanType, value);
    }

    /// <summary>
    /// Gets or sets the owner PAN number.
    /// </summary>
    public string OwnerPanNumber
    {
        get => _ownerPanNumber;
        set => SetProperty(ref _ownerPanNumber, value);
    }

    /// <summary>
    /// Gets or sets the owner first name.
    /// </summary>
    public string OwnerFirstName
    {
        get => _ownerFirstName;
        set => SetProperty(ref _ownerFirstName, value);
    }

    /// <summary>
    /// Gets or sets the owner middle name.
    /// </summary>
    public string OwnerMiddleName
    {
        get => _ownerMiddleName;
        set => SetProperty(ref _ownerMiddleName, value);
    }

    /// <summary>
    /// Gets or sets the owner last name.
    /// </summary>
    public string OwnerLastName
    {
        get => _ownerLastName;
        set => SetProperty(ref _ownerLastName, value);
    }

    /// <summary>
    /// Gets or sets the owner address.
    /// </summary>
    public string OwnerAddress
    {
        get => _ownerAddress;
        set => SetProperty(ref _ownerAddress, value);
    }

    /// <summary>
    /// Gets or sets the owner mobile number.
    /// </summary>
    public string OwnerMobileNumber
    {
        get => _ownerMobileNumber;
        set => SetProperty(ref _ownerMobileNumber, value);
    }

    /// <summary>
    /// Command to save the vehicle assignment.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSave))]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new AssignVehicleRequest
        {
            VehicleId = SelectedVehicleId,
            OwnerPanType = OwnerPanType,
            OwnerPanNumber = OwnerPanNumber,
            OwnerFirstName = OwnerFirstName,
            OwnerMiddleName = OwnerMiddleName,
            OwnerLastName = OwnerLastName,
            OwnerAddress = OwnerAddress,
            OwnerMobileNumber = OwnerMobileNumber,
            AssignDate = AssignDate
        };

        SetBusy("Creating vehicle assignment...");
        var result = await _vehicleAssignmentCommandService.AssignVehicleAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create vehicle assignment.";
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

    private bool CanExecuteSave()
    {
        return SelectedVehicleId > 0;
    }

    /// <summary>
    /// Loads all vehicles.
    /// </summary>
    private async Task LoadVehiclesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicles...");
        var vehicles = await _vehicleQueryService.GetAllVehiclesAsync(cancellationToken);
        UpdateVehicles(vehicles);
        ClearBusy();
    }

    /// <summary>
    /// Loads vehicle details when a vehicle is selected.
    /// </summary>
    private async Task LoadVehicleDetailsAsync()
    {
        if (SelectedVehicleId <= 0)
        {
            VehicleNumber = string.Empty;
            VehicleType = string.Empty;
            return;
        }

        var vehicle = await _vehicleQueryService.GetVehicleForEditAsync(SelectedVehicleId);
        if (vehicle != null)
        {
            VehicleNumber = vehicle.VehicleNumber;
            VehicleType = vehicle.VehicleType;
        }
    }

    private void UpdateVehicles(IEnumerable<VehicleListItem> vehicles)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            UpdateVehiclesInternal(vehicles);
        }
        else
        {
            dispatcher.Invoke(() => UpdateVehiclesInternal(vehicles));
        }
    }

    private void UpdateVehiclesInternal(IEnumerable<VehicleListItem> vehicles)
    {
        Vehicles.Clear();
        foreach (var vehicle in vehicles)
        {
            Vehicles.Add(vehicle);
        }
    }
}
