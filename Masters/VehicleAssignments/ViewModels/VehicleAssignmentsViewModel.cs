using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleAssignments.ViewModels;

/// <summary>
/// ViewModel for the Vehicle Assignments listing screen.
/// </summary>
public sealed partial class VehicleAssignmentsViewModel : ViewModelBase
{
    private readonly IVehicleAssignmentQueryService _vehicleAssignmentQueryService;
    private readonly IVehicleAssignmentCommandService _vehicleAssignmentCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private VehicleAssignmentListItem? _selectedAssignment;
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
    /// Initializes a new instance of the <see cref="VehicleAssignmentsViewModel"/> class.
    /// </summary>
    /// <param name="vehicleAssignmentQueryService">The vehicle assignment query service.</param>
    /// <param name="vehicleAssignmentCommandService">The vehicle assignment command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public VehicleAssignmentsViewModel(IVehicleAssignmentQueryService vehicleAssignmentQueryService, IVehicleAssignmentCommandService vehicleAssignmentCommandService, INavigationService navigationService)
    {
        _vehicleAssignmentQueryService = vehicleAssignmentQueryService ?? throw new ArgumentNullException(nameof(vehicleAssignmentQueryService));
        _vehicleAssignmentCommandService = vehicleAssignmentCommandService ?? throw new ArgumentNullException(nameof(vehicleAssignmentCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Vehicle Assignments";
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

        await LoadAssignmentsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadAssignmentsAsync(cancellationToken);
        
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
    /// Gets the collection of vehicle assignments to display.
    /// </summary>
    public ObservableCollection<VehicleAssignmentListItem> VehicleAssignments { get; } = new();

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
    /// Gets or sets the selected assignment.
    /// </summary>
    public VehicleAssignmentListItem? SelectedAssignment
    {
        get => _selectedAssignment;
        set
        {
            if (SetProperty(ref _selectedAssignment, value))
            {
                EditAssignmentCommand.NotifyCanExecuteChanged();
                ReleaseVehicleCommand.NotifyCanExecuteChanged();
                DeleteAssignmentCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the assignment list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadAssignmentsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Vehicle Assignment screen.
    /// </summary>
    [RelayCommand]
    private async Task AddAssignmentAsync()
    {
        await _navigationService.NavigateAsync<AddVehicleAssignmentViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Vehicle Assignment screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteAssignmentCommand))]
    private async Task EditAssignmentAsync()
    {
        if (SelectedAssignment is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["AssignmentId"] = SelectedAssignment.Id
        };

        await _navigationService.NavigateAsync<EditVehicleAssignmentViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to release the selected vehicle.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteAssignmentCommand))]
    private async Task ReleaseVehicleAsync()
    {
        if (SelectedAssignment is null)
        {
            return;
        }

        if (SelectedAssignment.Status != "Active")
        {
            ValidationError = "The vehicle has already been released.";
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to release this vehicle?",
            "Release Vehicle",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new ReleaseVehicleRequest
        {
            AssignmentId = SelectedAssignment.Id,
            ReleaseDate = DateTime.UtcNow
        };

        SetBusy("Releasing vehicle...");
        var result = await _vehicleAssignmentCommandService.ReleaseVehicleAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadAssignmentsAsync();
            SelectedAssignment = null;
            ReleaseVehicleCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to release vehicle.";
        }
    }

    /// <summary>
    /// Command to delete the selected assignment.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteAssignmentCommand))]
    private async Task DeleteAssignmentAsync()
    {
        if (SelectedAssignment is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this vehicle assignment?\n\nThis action hides the assignment from the application.",
            "Delete Vehicle Assignment",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteVehicleAssignmentRequest
        {
            AssignmentId = SelectedAssignment.Id
        };

        SetBusy("Deleting vehicle assignment...");
        var result = await _vehicleAssignmentCommandService.DeleteAssignmentAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadAssignmentsAsync();
            SelectedAssignment = null;
            DeleteAssignmentCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete vehicle assignment.";
        }
    }

    private bool CanExecuteAssignmentCommand()
    {
        return SelectedAssignment is not null;
    }

    /// <summary>
    /// Loads all vehicle assignments.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadAssignmentsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vehicle assignments...");
        var assignments = await _vehicleAssignmentQueryService.GetAllAssignmentsAsync(cancellationToken);
        UpdateAssignments(assignments);
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
            await SearchAssignmentsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches vehicle assignments based on the current search text.
    /// </summary>
    private async Task SearchAssignmentsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching vehicle assignments...");
        var assignments = await _vehicleAssignmentQueryService.SearchAssignmentsAsync(SearchText, cancellationToken);
        UpdateAssignments(assignments);
        ClearBusy();
    }

    /// <summary>
    /// Updates the vehicle assignments collection on the UI thread.
    /// </summary>
    /// <param name="assignments">The assignments to update.</param>
    private void UpdateAssignments(IEnumerable<VehicleAssignmentListItem> assignments)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateAssignmentsInternal(assignments);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateAssignmentsInternal(assignments));
        }
    }

    /// <summary>
    /// Updates the vehicle assignments collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="assignments">The assignments to update.</param>
    private void UpdateAssignmentsInternal(IEnumerable<VehicleAssignmentListItem> assignments)
    {
        VehicleAssignments.Clear();
        foreach (var assignment in assignments)
        {
            VehicleAssignments.Add(assignment);
        }
    }
}
