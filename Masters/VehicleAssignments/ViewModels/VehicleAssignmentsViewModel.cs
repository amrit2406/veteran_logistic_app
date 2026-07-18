using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleAssignments.ViewModels;

/// <summary>
/// ViewModel for the Vehicle Assignment enquiry screen.
/// </summary>
public sealed partial class VehicleAssignmentsViewModel : ViewModelBase
{
    private readonly IVehicleAssignmentQueryService _vehicleAssignmentQueryService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private string _validationError = string.Empty;
    private VehicleAssignmentModel? _vehicleAssignment;
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
    /// Initializes a new instance of the <see cref="VehicleAssignmentsViewModel"/> class.
    /// </summary>
    /// <param name="vehicleAssignmentQueryService">The vehicle assignment query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public VehicleAssignmentsViewModel(IVehicleAssignmentQueryService vehicleAssignmentQueryService, INavigationService navigationService)
    {
        _vehicleAssignmentQueryService = vehicleAssignmentQueryService ?? throw new ArgumentNullException(nameof(vehicleAssignmentQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Vehicle Assignment";
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        GoBackCommand.NotifyCanExecuteChanged();
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
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
    /// Gets or sets the search text (vehicle number).
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
    /// Gets or sets the vehicle assignment details.
    /// </summary>
    public VehicleAssignmentModel? VehicleAssignment
    {
        get => _vehicleAssignment;
        set => SetProperty(ref _vehicleAssignment, value);
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
    /// Gets a value indicating whether a vehicle is found.
    /// </summary>
    public bool VehicleFound => VehicleAssignment is not null;

    /// <summary>
    /// Gets a value indicating whether no vehicle is found message should be shown.
    /// </summary>
    public bool ShowNoVehicleFound => !string.IsNullOrWhiteSpace(SearchText) && VehicleAssignment is null && !IsBusy;

    /// <summary>
    /// Command to search for a vehicle by vehicle number.
    /// </summary>
    [RelayCommand]
    private async Task SearchVehicleAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            ValidationError = "Please enter a vehicle number.";
            return;
        }

        ValidationError = string.Empty;
        VehicleAssignment = null;

        SetBusy("Searching vehicle...");
        var result = await _vehicleAssignmentQueryService.GetVehicleAssignmentAsync(SearchText.Trim(), CancellationToken.None);
        ClearBusy();

        VehicleAssignment = result;
        OnPropertyChanged(nameof(VehicleFound));
        OnPropertyChanged(nameof(ShowNoVehicleFound));

        if (result is null)
        {
            ValidationError = "No vehicle found.";
        }
    }

    /// <summary>
    /// Command to clear the screen.
    /// </summary>
    [RelayCommand]
    private void ClearScreen()
    {
        SearchText = string.Empty;
        VehicleAssignment = null;
        ValidationError = string.Empty;
        OnPropertyChanged(nameof(VehicleFound));
        OnPropertyChanged(nameof(ShowNoVehicleFound));
    }

    /// <summary>
    /// Command to refresh the current search.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            await SearchVehicleAsync();
        }
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
            // Wait 500ms to allow user to finish typing
            await Task.Delay(500, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                await SearchVehicleAsync();
            }
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }
}
