using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.FuelPumps.ViewModels;

/// <summary>
/// ViewModel for the Fuel Pumps listing screen.
/// </summary>
public sealed partial class FuelPumpsViewModel : ViewModelBase
{
    private readonly IFuelPumpQueryService _fuelPumpQueryService;
    private readonly IFuelPumpCommandService _fuelPumpCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private FuelPumpListItem? _selectedFuelPump;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuelPumpsViewModel"/> class.
    /// </summary>
    /// <param name="fuelPumpQueryService">The fuel pump query service.</param>
    /// <param name="fuelPumpCommandService">The fuel pump command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public FuelPumpsViewModel(IFuelPumpQueryService fuelPumpQueryService, IFuelPumpCommandService fuelPumpCommandService, INavigationService navigationService)
    {
        _fuelPumpQueryService = fuelPumpQueryService ?? throw new ArgumentNullException(nameof(fuelPumpQueryService));
        _fuelPumpCommandService = fuelPumpCommandService ?? throw new ArgumentNullException(nameof(fuelPumpCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Fuel Pumps";
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

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadFuelPumpsAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of fuel pumps to display.
    /// </summary>
    public ObservableCollection<FuelPumpListItem> FuelPumps { get; } = new();

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
    /// Gets or sets the selected fuel pump.
    /// </summary>
    public FuelPumpListItem? SelectedFuelPump
    {
        get => _selectedFuelPump;
        set
        {
            if (SetProperty(ref _selectedFuelPump, value))
            {
                EditFuelPumpCommand.NotifyCanExecuteChanged();
                ActivateFuelPumpCommand.NotifyCanExecuteChanged();
                DeactivateFuelPumpCommand.NotifyCanExecuteChanged();
                DeleteFuelPumpCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the fuel pump list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadFuelPumpsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Fuel Pump screen.
    /// </summary>
    [RelayCommand]
    private async Task AddFuelPumpAsync()
    {
        await _navigationService.NavigateAsync<AddFuelPumpViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Fuel Pump screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteFuelPumpCommand))]
    private async Task EditFuelPumpAsync()
    {
        if (SelectedFuelPump is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["FuelPumpId"] = SelectedFuelPump.Id
        };

        await _navigationService.NavigateAsync<EditFuelPumpViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected fuel pump.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteFuelPumpCommand))]
    private async Task ActivateFuelPumpAsync()
    {
        if (SelectedFuelPump is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateFuelPumpStatusRequest
        {
            FuelPumpId = SelectedFuelPump.Id,
            IsActive = true
        };

        SetBusy("Activating fuel pump...");
        var result = await _fuelPumpCommandService.UpdateFuelPumpStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleFuelPumpStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate fuel pump.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected fuel pump.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteFuelPumpCommand))]
    private async Task DeactivateFuelPumpAsync()
    {
        if (SelectedFuelPump is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateFuelPumpStatusRequest
        {
            FuelPumpId = SelectedFuelPump.Id,
            IsActive = false
        };

        SetBusy("Deactivating fuel pump...");
        var result = await _fuelPumpCommandService.UpdateFuelPumpStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleFuelPumpStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate fuel pump.";
        }
    }

    private async Task HandleFuelPumpStatusUpdateSuccess()
    {
        await LoadFuelPumpsAsync();
        SelectedFuelPump = null;
        ActivateFuelPumpCommand.NotifyCanExecuteChanged();
        DeactivateFuelPumpCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected fuel pump.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteFuelPumpCommand))]
    private async Task DeleteFuelPumpAsync()
    {
        if (SelectedFuelPump is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this fuel pump?\n\nThis action hides the fuel pump from the application.",
            "Delete Fuel Pump",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteFuelPumpRequest
        {
            FuelPumpId = SelectedFuelPump.Id
        };

        SetBusy("Deleting fuel pump...");
        var result = await _fuelPumpCommandService.DeleteFuelPumpAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadFuelPumpsAsync();
            SelectedFuelPump = null;
            DeleteFuelPumpCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete fuel pump.";
        }
    }

    private bool CanExecuteFuelPumpCommand()
    {
        return SelectedFuelPump is not null;
    }

    /// <summary>
    /// Loads all fuel pumps.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadFuelPumpsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading fuel pumps...");
        var fuelPumps = await _fuelPumpQueryService.GetAllFuelPumpsAsync(cancellationToken);
        UpdateFuelPumps(fuelPumps);
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
            await SearchFuelPumpsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches fuel pumps based on the current search text.
    /// </summary>
    private async Task SearchFuelPumpsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching fuel pumps...");
        var fuelPumps = await _fuelPumpQueryService.SearchFuelPumpsAsync(SearchText, cancellationToken);
        UpdateFuelPumps(fuelPumps);
        ClearBusy();
    }

    /// <summary>
    /// Updates the fuel pumps collection on the UI thread.
    /// </summary>
    /// <param name="fuelPumps">The fuel pumps to update.</param>
    private void UpdateFuelPumps(IReadOnlyList<FuelPumpListItem> fuelPumps)
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
    private void UpdateFuelPumpsInternal(IReadOnlyList<FuelPumpListItem> fuelPumps)
    {
        FuelPumps.Clear();
        foreach (var fuelPump in fuelPumps)
        {
            FuelPumps.Add(fuelPump);
        }
    }
}
