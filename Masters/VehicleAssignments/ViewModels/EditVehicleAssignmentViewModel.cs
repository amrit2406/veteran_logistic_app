using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleAssignments.ViewModels;

/// <summary>
/// ViewModel for the Edit Vehicle Assignment screen.
/// </summary>
public sealed partial class EditVehicleAssignmentViewModel : ViewModelBase, INavigationAware
{
    private readonly IVehicleAssignmentQueryService _vehicleAssignmentQueryService;
    private readonly IVehicleAssignmentCommandService _vehicleAssignmentCommandService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IVehicleOwnerQueryService _vehicleOwnerQueryService;
    private readonly INavigationService _navigationService;
    private int _assignmentId;
    private int _selectedVehicleId;
    private int _selectedVehicleOwnerId;
    private DateTime _assignDate;
    private DateTime? _releaseDate;
    private bool _isActive;
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
    /// Initializes a new instance of the <see cref="EditVehicleAssignmentViewModel"/> class.
    /// </summary>
    /// <param name="vehicleAssignmentQueryService">The vehicle assignment query service.</param>
    /// <param name="vehicleAssignmentCommandService">The vehicle assignment command service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="vehicleOwnerQueryService">The vehicle owner query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditVehicleAssignmentViewModel(
        IVehicleAssignmentQueryService vehicleAssignmentQueryService,
        IVehicleAssignmentCommandService vehicleAssignmentCommandService,
        IVehicleQueryService vehicleQueryService,
        IVehicleOwnerQueryService vehicleOwnerQueryService,
        INavigationService navigationService)
    {
        _vehicleAssignmentQueryService = vehicleAssignmentQueryService ?? throw new ArgumentNullException(nameof(vehicleAssignmentQueryService));
        _vehicleAssignmentCommandService = vehicleAssignmentCommandService ?? throw new ArgumentNullException(nameof(vehicleAssignmentCommandService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _vehicleOwnerQueryService = vehicleOwnerQueryService ?? throw new ArgumentNullException(nameof(vehicleOwnerQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Vehicle Assignment";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadVehiclesAsync(cancellationToken);
        await LoadVehicleOwnersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        if (_assignmentId > 0)
        {
            await LoadAssignmentAsync(cancellationToken);
        }
        await base.OnNavigatedToAsync(cancellationToken);
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter != null && parameter.TryGetValue<int>("AssignmentId", out var assignmentId))
        {
            _assignmentId = assignmentId;
        }
    }

    /// <summary>
    /// Gets the collection of vehicles for selection.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets the collection of vehicle owners for selection.
    /// </summary>
    public ObservableCollection<VehicleOwnerListItem> VehicleOwners { get; } = new();

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
    /// Gets or sets the selected vehicle owner ID.
    /// </summary>
    public int SelectedVehicleOwnerId
    {
        get => _selectedVehicleOwnerId;
        set
        {
            if (SetProperty(ref _selectedVehicleOwnerId, value))
            {
                _ = LoadOwnerDetailsAsync();
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
    /// Gets or sets whether the assignment is active.
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
    /// Gets or sets the owner PAN type (read-only, populated from selected owner).
    /// </summary>
    public string OwnerPanType
    {
        get => _ownerPanType;
        set => SetProperty(ref _ownerPanType, value);
    }

    /// <summary>
    /// Gets or sets the owner PAN number (read-only, populated from selected owner).
    /// </summary>
    public string OwnerPanNumber
    {
        get => _ownerPanNumber;
        set => SetProperty(ref _ownerPanNumber, value);
    }

    /// <summary>
    /// Gets or sets the owner first name (read-only, populated from selected owner).
    /// </summary>
    public string OwnerFirstName
    {
        get => _ownerFirstName;
        set => SetProperty(ref _ownerFirstName, value);
    }

    /// <summary>
    /// Gets or sets the owner middle name (read-only, populated from selected owner).
    /// </summary>
    public string OwnerMiddleName
    {
        get => _ownerMiddleName;
        set => SetProperty(ref _ownerMiddleName, value);
    }

    /// <summary>
    /// Gets or sets the owner last name (read-only, populated from selected owner).
    /// </summary>
    public string OwnerLastName
    {
        get => _ownerLastName;
        set => SetProperty(ref _ownerLastName, value);
    }

    /// <summary>
    /// Gets or sets the owner address (read-only, populated from selected owner).
    /// </summary>
    public string OwnerAddress
    {
        get => _ownerAddress;
        set => SetProperty(ref _ownerAddress, value);
    }

    /// <summary>
    /// Gets or sets the owner mobile number (read-only, populated from selected owner).
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

        var request = new UpdateVehicleAssignmentRequest
        {
            AssignmentId = _assignmentId,
            VehicleId = SelectedVehicleId,
            VehicleOwnerId = SelectedVehicleOwnerId,
            AssignDate = AssignDate,
            ReleaseDate = ReleaseDate,
            IsActive = IsActive
        };

        SetBusy("Updating vehicle assignment...");
        var result = await _vehicleAssignmentCommandService.UpdateAssignmentAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update vehicle assignment.";
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
        return SelectedVehicleId > 0 && SelectedVehicleOwnerId > 0;
    }

    /// <summary>
    /// Loads the assignment details.
    /// </summary>
    private async Task LoadAssignmentAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicle assignment...");
        var assignment = await _vehicleAssignmentQueryService.GetAssignmentForEditAsync(_assignmentId, cancellationToken);
        
        if (assignment != null)
        {
            SelectedVehicleId = assignment.VehicleId;
            SelectedVehicleOwnerId = assignment.VehicleOwnerId;
            AssignDate = assignment.AssignDate;
            ReleaseDate = assignment.ReleaseDate;
            IsActive = assignment.IsActive;
            VehicleNumber = assignment.VehicleNumber;
            VehicleType = assignment.VehicleType;
            OwnerPanType = assignment.PANType;
            OwnerPanNumber = assignment.PANNumber;
            OwnerFirstName = assignment.FirstName;
            OwnerMiddleName = assignment.MiddleName;
            OwnerLastName = assignment.LastName;
            OwnerAddress = assignment.Address;
            OwnerMobileNumber = assignment.MobileNumber;
        }
        
        ClearBusy();
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
    /// Loads all vehicle owners.
    /// </summary>
    private async Task LoadVehicleOwnersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicle owners...");
        var owners = await _vehicleOwnerQueryService.GetAllVehicleOwnersAsync(cancellationToken);
        UpdateVehicleOwners(owners);
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

    /// <summary>
    /// Loads owner details when a vehicle owner is selected.
    /// </summary>
    private async Task LoadOwnerDetailsAsync()
    {
        if (SelectedVehicleOwnerId <= 0)
        {
            OwnerPanType = string.Empty;
            OwnerPanNumber = string.Empty;
            OwnerFirstName = string.Empty;
            OwnerMiddleName = string.Empty;
            OwnerLastName = string.Empty;
            OwnerAddress = string.Empty;
            OwnerMobileNumber = string.Empty;
            return;
        }

        var owner = await _vehicleOwnerQueryService.GetVehicleOwnerForEditAsync(SelectedVehicleOwnerId);
        if (owner != null)
        {
            OwnerPanType = owner.PANType;
            OwnerPanNumber = owner.PANNumber;
            OwnerFirstName = owner.FirstName;
            OwnerMiddleName = owner.MiddleName;
            OwnerLastName = owner.LastName;
            OwnerAddress = owner.Address;
            OwnerMobileNumber = owner.Mobile;
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

    private void UpdateVehicleOwners(IEnumerable<VehicleOwnerListItem> owners)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            UpdateVehicleOwnersInternal(owners);
        }
        else
        {
            dispatcher.Invoke(() => UpdateVehicleOwnersInternal(owners));
        }
    }

    private void UpdateVehicleOwnersInternal(IEnumerable<VehicleOwnerListItem> owners)
    {
        VehicleOwners.Clear();
        foreach (var owner in owners)
        {
            VehicleOwners.Add(owner);
        }
    }
}
