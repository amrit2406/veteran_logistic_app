using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.SourceDestinations.ViewModels;

/// <summary>
/// ViewModel for the Source/Destinations listing screen.
/// </summary>
public sealed partial class SourceDestinationsViewModel : ViewModelBase
{
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly ISourceDestinationCommandService _sourceDestinationCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private SourceDestinationListItem? _selectedSourceDestination;
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
    /// Initializes a new instance of the <see cref="SourceDestinationsViewModel"/> class.
    /// </summary>
    /// <param name="sourceDestinationQueryService">The source/destination query service.</param>
    /// <param name="sourceDestinationCommandService">The source/destination command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public SourceDestinationsViewModel(ISourceDestinationQueryService sourceDestinationQueryService, ISourceDestinationCommandService sourceDestinationCommandService, INavigationService navigationService)
    {
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _sourceDestinationCommandService = sourceDestinationCommandService ?? throw new ArgumentNullException(nameof(sourceDestinationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Source/Destinations";
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

        await LoadSourceDestinationsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadSourceDestinationsAsync(cancellationToken);
        
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
    /// Gets the collection of source/destinations to display.
    /// </summary>
    public ObservableCollection<SourceDestinationListItem> SourceDestinations { get; } = new();

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
    /// Gets or sets the selected source/destination.
    /// </summary>
    public SourceDestinationListItem? SelectedSourceDestination
    {
        get => _selectedSourceDestination;
        set
        {
            if (SetProperty(ref _selectedSourceDestination, value))
            {
                EditSourceDestinationCommand.NotifyCanExecuteChanged();
                ActivateSourceDestinationCommand.NotifyCanExecuteChanged();
                DeactivateSourceDestinationCommand.NotifyCanExecuteChanged();
                DeleteSourceDestinationCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the source/destination list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSourceDestinationsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Source/Destination screen.
    /// </summary>
    [RelayCommand]
    private async Task AddSourceDestinationAsync()
    {
        await _navigationService.NavigateAsync<AddSourceDestinationViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Source/Destination screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceDestinationCommand))]
    private async Task EditSourceDestinationAsync()
    {
        if (SelectedSourceDestination is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["SourceDestinationId"] = SelectedSourceDestination.Id
        };

        await _navigationService.NavigateAsync<EditSourceDestinationViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected source/destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceDestinationCommand))]
    private async Task ActivateSourceDestinationAsync()
    {
        if (SelectedSourceDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateSourceDestinationStatusRequest
        {
            SourceDestinationId = SelectedSourceDestination.Id,
            IsActive = true
        };

        SetBusy("Activating source/destination...");
        var result = await _sourceDestinationCommandService.UpdateSourceDestinationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleSourceDestinationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate source/destination.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected source/destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceDestinationCommand))]
    private async Task DeactivateSourceDestinationAsync()
    {
        if (SelectedSourceDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateSourceDestinationStatusRequest
        {
            SourceDestinationId = SelectedSourceDestination.Id,
            IsActive = false
        };

        SetBusy("Deactivating source/destination...");
        var result = await _sourceDestinationCommandService.UpdateSourceDestinationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleSourceDestinationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate source/destination.";
        }
    }

    private async Task HandleSourceDestinationStatusUpdateSuccess()
    {
        await LoadSourceDestinationsAsync();
        SelectedSourceDestination = null;
        ActivateSourceDestinationCommand.NotifyCanExecuteChanged();
        DeactivateSourceDestinationCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected source/destination.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceDestinationCommand))]
    private async Task DeleteSourceDestinationAsync()
    {
        if (SelectedSourceDestination is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this source/destination?\n\nThis action hides the source/destination from the application.",
            "Delete Source/Destination",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteSourceDestinationRequest
        {
            SourceDestinationId = SelectedSourceDestination.Id
        };

        SetBusy("Deleting source/destination...");
        var result = await _sourceDestinationCommandService.DeleteSourceDestinationAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadSourceDestinationsAsync();
            SelectedSourceDestination = null;
            DeleteSourceDestinationCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete source/destination.";
        }
    }

    private bool CanExecuteSourceDestinationCommand()
    {
        return SelectedSourceDestination is not null;
    }

    /// <summary>
    /// Loads all source/destinations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadSourceDestinationsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading source/destinations...");
        var sourceDestinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken);
        UpdateSourceDestinations(sourceDestinations);
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
            await SearchSourceDestinationsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches source/destinations based on the current search text.
    /// </summary>
    private async Task SearchSourceDestinationsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching source/destinations...");
        var sourceDestinations = await _sourceDestinationQueryService.SearchSourceDestinationsAsync(SearchText, cancellationToken);
        UpdateSourceDestinations(sourceDestinations);
        ClearBusy();
    }

    /// <summary>
    /// Updates the source/destinations collection on the UI thread.
    /// </summary>
    /// <param name="sourceDestinations">The source/destinations to update.</param>
    private void UpdateSourceDestinations(IEnumerable<SourceDestinationListItem> sourceDestinations)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateSourceDestinationsInternal(sourceDestinations);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateSourceDestinationsInternal(sourceDestinations));
        }
    }

    /// <summary>
    /// Updates the source/destinations collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="sourceDestinations">The source/destinations to update.</param>
    private void UpdateSourceDestinationsInternal(IEnumerable<SourceDestinationListItem> sourceDestinations)
    {
        SourceDestinations.Clear();
        foreach (var sourceDestination in sourceDestinations)
        {
            SourceDestinations.Add(sourceDestination);
        }
    }
}
