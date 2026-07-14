using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Vehicles.ViewModels;

/// <summary>
/// ViewModel for the Vehicles listing screen.
/// </summary>
public sealed partial class VehiclesViewModel : ViewModelBase
{
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IVehicleCommandService _vehicleCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private VehicleListItem? _selectedVehicle;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclesViewModel"/> class.
    /// </summary>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="vehicleCommandService">The vehicle command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public VehiclesViewModel(IVehicleQueryService vehicleQueryService, IVehicleCommandService vehicleCommandService, INavigationService navigationService)
    {
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _vehicleCommandService = vehicleCommandService ?? throw new ArgumentNullException(nameof(vehicleCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Vehicles";
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        GoBackCommand.NotifyCanExecuteChanged();
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

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadVehiclesAsync(cancellationToken);
        
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher != null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(() =>
            {
                GoBackCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CanGoBack));
            });
        }
        else
        {
            GoBackCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanGoBack));
        }
    }

    /// <summary>
    /// Gets the collection of vehicles to display.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets or sets the search text.
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _ = DebouncedSearchAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected vehicle.
    /// </summary>
    public VehicleListItem? SelectedVehicle
    {
        get => _selectedVehicle;
        set
        {
            if (SetProperty(ref _selectedVehicle, value))
            {
                EditVehicleCommand.NotifyCanExecuteChanged();
                ActivateVehicleCommand.NotifyCanExecuteChanged();
                DeactivateVehicleCommand.NotifyCanExecuteChanged();
                DeleteVehicleCommand.NotifyCanExecuteChanged();
            }
        }
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
    /// Command to refresh the vehicle list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadVehiclesAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Vehicle screen.
    /// </summary>
    [RelayCommand]
    private async Task AddVehicleAsync()
    {
        await _navigationService.NavigateAsync<AddVehicleViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Vehicle screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleCommand))]
    private async Task EditVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["VehicleId"] = SelectedVehicle.Id
        };

        await _navigationService.NavigateAsync<EditVehicleViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected vehicle.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleCommand))]
    private async Task ActivateVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVehicleStatusRequest
        {
            VehicleId = SelectedVehicle.Id,
            IsActive = true
        };

        SetBusy("Activating vehicle...");
        var result = await _vehicleCommandService.UpdateVehicleStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVehicleStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate vehicle.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected vehicle.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleCommand))]
    private async Task DeactivateVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVehicleStatusRequest
        {
            VehicleId = SelectedVehicle.Id,
            IsActive = false
        };

        SetBusy("Deactivating vehicle...");
        var result = await _vehicleCommandService.UpdateVehicleStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVehicleStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate vehicle.";
        }
    }

    private async Task HandleVehicleStatusUpdateSuccess()
    {
        await LoadVehiclesAsync();
        SelectedVehicle = null;
        ActivateVehicleCommand.NotifyCanExecuteChanged();
        DeactivateVehicleCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected vehicle.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleCommand))]
    private async Task DeleteVehicleAsync()
    {
        if (SelectedVehicle is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this vehicle?\n\nThis action hides the vehicle from the application.",
            "Delete Vehicle",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteVehicleRequest
        {
            VehicleId = SelectedVehicle.Id
        };

        SetBusy("Deleting vehicle...");
        var result = await _vehicleCommandService.DeleteVehicleAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadVehiclesAsync();
            SelectedVehicle = null;
            DeleteVehicleCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete vehicle.";
        }
    }

    private bool CanExecuteVehicleCommand()
    {
        return SelectedVehicle is not null;
    }

    /// <summary>
    /// Loads all vehicles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadVehiclesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicles...");
        var vehicles = await _vehicleQueryService.GetAllVehiclesAsync(cancellationToken);
        UpdateVehicles(vehicles);
        ClearBusy();
    }

    /// <summary>
    /// Debounced search to prevent excessive database queries.
    /// </summary>
    private async Task DebouncedSearchAsync()
    {
        // Cancel and dispose previous search if still running
        var cts = _searchCancellationTokenSource;
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }

        _searchCancellationTokenSource = new CancellationTokenSource();
        var token = _searchCancellationTokenSource.Token;

        try
        {
            // Wait 300ms to allow user to finish typing
            await Task.Delay(300, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            await SearchVehiclesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches vehicles based on the current search text.
    /// </summary>
    private async Task SearchVehiclesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching vehicles...");
        var vehicles = await _vehicleQueryService.SearchVehiclesAsync(SearchText, cancellationToken);
        UpdateVehicles(vehicles);
        ClearBusy();
    }

    /// <summary>
    /// Updates the vehicles collection on the UI thread.
    /// </summary>
    /// <param name="vehicles">The vehicles to update.</param>
    private void UpdateVehicles(IEnumerable<VehicleListItem> vehicles)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateVehiclesInternal(vehicles);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateVehiclesInternal(vehicles));
        }
    }

    /// <summary>
    /// Updates the vehicles collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="vehicles">The vehicles to update.</param>
    private void UpdateVehiclesInternal(IEnumerable<VehicleListItem> vehicles)
    {
        Vehicles.Clear();
        foreach (var vehicle in vehicles)
        {
            Vehicles.Add(vehicle);
        }
    }
}
