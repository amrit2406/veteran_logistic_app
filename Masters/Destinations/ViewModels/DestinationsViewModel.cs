using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Destinations.ViewModels;

/// <summary>
/// ViewModel for the Destinations listing screen.
/// </summary>
public sealed partial class DestinationsViewModel : ViewModelBase
{
    private readonly IDestinationQueryService _destinationQueryService;
    private readonly IDestinationCommandService _destinationCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private DestinationListItem? _selectedDestination;
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
    /// Initializes a new instance of the <see cref="DestinationsViewModel"/> class.
    /// </summary>
    /// <param name="destinationQueryService">The destination query service.</param>
    /// <param name="destinationCommandService">The destination command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public DestinationsViewModel(IDestinationQueryService destinationQueryService, IDestinationCommandService destinationCommandService, INavigationService navigationService)
    {
        _destinationQueryService = destinationQueryService ?? throw new ArgumentNullException(nameof(destinationQueryService));
        _destinationCommandService = destinationCommandService ?? throw new ArgumentNullException(nameof(destinationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Destinations";
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

        await LoadDestinationsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadDestinationsAsync(cancellationToken);
        GoBackCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanGoBack));
    }

    /// <summary>
    /// Gets the collection of destinations to display.
    /// </summary>
    public ObservableCollection<DestinationListItem> Destinations { get; } = new();

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
    /// Gets or sets the selected destination.
    /// </summary>
    public DestinationListItem? SelectedDestination
    {
        get => _selectedDestination;
        set
        {
            if (SetProperty(ref _selectedDestination, value))
            {
                EditDestinationCommand.NotifyCanExecuteChanged();
                ActivateDestinationCommand.NotifyCanExecuteChanged();
                DeactivateDestinationCommand.NotifyCanExecuteChanged();
                DeleteDestinationCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the destination list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDestinationsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Destination screen.
    /// </summary>
    [RelayCommand]
    private async Task AddDestinationAsync()
    {
        await _navigationService.NavigateAsync<AddDestinationViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Destination screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDestinationCommand))]
    private async Task EditDestinationAsync()
    {
        if (SelectedDestination is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["DestinationId"] = SelectedDestination.Id
        };

        await _navigationService.NavigateAsync<EditDestinationViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDestinationCommand))]
    private async Task ActivateDestinationAsync()
    {
        if (SelectedDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateDestinationStatusRequest
        {
            DestinationId = SelectedDestination.Id,
            IsActive = true
        };

        SetBusy("Activating destination...");
        var result = await _destinationCommandService.UpdateDestinationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleDestinationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate destination.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDestinationCommand))]
    private async Task DeactivateDestinationAsync()
    {
        if (SelectedDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateDestinationStatusRequest
        {
            DestinationId = SelectedDestination.Id,
            IsActive = false
        };

        SetBusy("Deactivating destination...");
        var result = await _destinationCommandService.UpdateDestinationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleDestinationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate destination.";
        }
    }

    private async Task HandleDestinationStatusUpdateSuccess()
    {
        await LoadDestinationsAsync();
        SelectedDestination = null;
        ActivateDestinationCommand.NotifyCanExecuteChanged();
        DeactivateDestinationCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDestinationCommand))]
    private async Task DeleteDestinationAsync()
    {
        if (SelectedDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this destination?\n\nThis action hides the destination from the application.",
            "Delete Destination",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteDestinationRequest
        {
            DestinationId = SelectedDestination.Id
        };

        SetBusy("Deleting destination...");
        var result = await _destinationCommandService.DeleteDestinationAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadDestinationsAsync();
            SelectedDestination = null;
            DeleteDestinationCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete destination.";
        }
    }

    private bool CanExecuteDestinationCommand()
    {
        return SelectedDestination is not null;
    }

    /// <summary>
    /// Loads all destinations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadDestinationsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading destinations...");
        var destinations = await _destinationQueryService.GetAllDestinationsAsync(cancellationToken);
        UpdateDestinations(destinations);
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
            await SearchDestinationsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches destinations based on the current search text.
    /// </summary>
    private async Task SearchDestinationsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching destinations...");
        var destinations = await _destinationQueryService.SearchDestinationsAsync(SearchText, cancellationToken);
        UpdateDestinations(destinations);
        ClearBusy();
    }

    /// <summary>
    /// Updates the destinations collection on the UI thread.
    /// </summary>
    /// <param name="destinations">The destinations to update.</param>
    private void UpdateDestinations(IEnumerable<DestinationListItem> destinations)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateDestinationsInternal(destinations);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateDestinationsInternal(destinations));
        }
    }

    /// <summary>
    /// Updates the destinations collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="destinations">The destinations to update.</param>
    private void UpdateDestinationsInternal(IEnumerable<DestinationListItem> destinations)
    {
        Destinations.Clear();
        foreach (var destination in destinations)
        {
            Destinations.Add(destination);
        }
    }
}
