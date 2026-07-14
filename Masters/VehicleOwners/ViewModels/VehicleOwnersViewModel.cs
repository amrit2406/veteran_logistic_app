using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleOwners.ViewModels;

/// <summary>
/// ViewModel for the Vehicle Owners listing screen.
/// </summary>
public sealed partial class VehicleOwnersViewModel : ViewModelBase
{
    private readonly IVehicleOwnerQueryService _vehicleOwnerQueryService;
    private readonly IVehicleOwnerCommandService _vehicleOwnerCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private VehicleOwnerListItem? _selectedVehicleOwner;
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
    /// Initializes a new instance of the <see cref="VehicleOwnersViewModel"/> class.
    /// </summary>
    /// <param name="vehicleOwnerQueryService">The vehicle owner query service.</param>
    /// <param name="vehicleOwnerCommandService">The vehicle owner command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public VehicleOwnersViewModel(IVehicleOwnerQueryService vehicleOwnerQueryService, IVehicleOwnerCommandService vehicleOwnerCommandService, INavigationService navigationService)
    {
        _vehicleOwnerQueryService = vehicleOwnerQueryService ?? throw new ArgumentNullException(nameof(vehicleOwnerQueryService));
        _vehicleOwnerCommandService = vehicleOwnerCommandService ?? throw new ArgumentNullException(nameof(vehicleOwnerCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Vehicle Owners";
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

        await LoadVehicleOwnersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadVehicleOwnersAsync(cancellationToken);
        
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
    /// Gets the collection of vehicle owners to display.
    /// </summary>
    public ObservableCollection<VehicleOwnerListItem> VehicleOwners { get; } = new();

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
    /// Gets or sets the selected vehicle owner.
    /// </summary>
    public VehicleOwnerListItem? SelectedVehicleOwner
    {
        get => _selectedVehicleOwner;
        set
        {
            if (SetProperty(ref _selectedVehicleOwner, value))
            {
                EditVehicleOwnerCommand.NotifyCanExecuteChanged();
                ActivateVehicleOwnerCommand.NotifyCanExecuteChanged();
                DeactivateVehicleOwnerCommand.NotifyCanExecuteChanged();
                DeleteVehicleOwnerCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the vehicle owner list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadVehicleOwnersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Vehicle Owner screen.
    /// </summary>
    [RelayCommand]
    private async Task AddVehicleOwnerAsync()
    {
        await _navigationService.NavigateAsync<AddVehicleOwnerViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Vehicle Owner screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleOwnerCommand))]
    private async Task EditVehicleOwnerAsync()
    {
        if (SelectedVehicleOwner is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["VehicleOwnerId"] = SelectedVehicleOwner.Id
        };

        await _navigationService.NavigateAsync<EditVehicleOwnerViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected vehicle owner.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleOwnerCommand))]
    private async Task ActivateVehicleOwnerAsync()
    {
        if (SelectedVehicleOwner is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVehicleOwnerStatusRequest
        {
            VehicleOwnerId = SelectedVehicleOwner.Id,
            IsActive = true
        };

        SetBusy("Activating vehicle owner...");
        var result = await _vehicleOwnerCommandService.UpdateVehicleOwnerStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVehicleOwnerStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate vehicle owner.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected vehicle owner.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleOwnerCommand))]
    private async Task DeactivateVehicleOwnerAsync()
    {
        if (SelectedVehicleOwner is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVehicleOwnerStatusRequest
        {
            VehicleOwnerId = SelectedVehicleOwner.Id,
            IsActive = false
        };

        SetBusy("Deactivating vehicle owner...");
        var result = await _vehicleOwnerCommandService.UpdateVehicleOwnerStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVehicleOwnerStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate vehicle owner.";
        }
    }

    private async Task HandleVehicleOwnerStatusUpdateSuccess()
    {
        await LoadVehicleOwnersAsync();
        SelectedVehicleOwner = null;
        ActivateVehicleOwnerCommand.NotifyCanExecuteChanged();
        DeactivateVehicleOwnerCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected vehicle owner.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVehicleOwnerCommand))]
    private async Task DeleteVehicleOwnerAsync()
    {
        if (SelectedVehicleOwner is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this vehicle owner?\n\nThis action hides the vehicle owner from the application.",
            "Delete Vehicle Owner",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteVehicleOwnerRequest
        {
            VehicleOwnerId = SelectedVehicleOwner.Id
        };

        SetBusy("Deleting vehicle owner...");
        var result = await _vehicleOwnerCommandService.DeleteVehicleOwnerAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadVehicleOwnersAsync();
            SelectedVehicleOwner = null;
            DeleteVehicleOwnerCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete vehicle owner.";
        }
    }

    private bool CanExecuteVehicleOwnerCommand()
    {
        return SelectedVehicleOwner is not null;
    }

    /// <summary>
    /// Loads all vehicle owners.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadVehicleOwnersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicle owners...");
        var vehicleOwners = await _vehicleOwnerQueryService.GetAllVehicleOwnersAsync(cancellationToken);
        UpdateVehicleOwners(vehicleOwners);
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
            await SearchVehicleOwnersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches vehicle owners based on the current search text.
    /// </summary>
    private async Task SearchVehicleOwnersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching vehicle owners...");
        var vehicleOwners = await _vehicleOwnerQueryService.SearchVehicleOwnersAsync(SearchText, cancellationToken);
        UpdateVehicleOwners(vehicleOwners);
        ClearBusy();
    }

    /// <summary>
    /// Updates the vehicle owners collection on the UI thread.
    /// </summary>
    /// <param name="vehicleOwners">The vehicle owners to update.</param>
    private void UpdateVehicleOwners(IEnumerable<VehicleOwnerListItem> vehicleOwners)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateVehicleOwnersInternal(vehicleOwners);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateVehicleOwnersInternal(vehicleOwners));
        }
    }

    /// <summary>
    /// Updates the vehicle owners collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="vehicleOwners">The vehicle owners to update.</param>
    private void UpdateVehicleOwnersInternal(IEnumerable<VehicleOwnerListItem> vehicleOwners)
    {
        VehicleOwners.Clear();
        foreach (var vehicleOwner in vehicleOwners)
        {
            VehicleOwners.Add(vehicleOwner);
        }
    }
}
