using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Sources.ViewModels;

/// <summary>
/// ViewModel for the Sources listing screen.
/// </summary>
public sealed partial class SourcesViewModel : ViewModelBase
{
    private readonly ISourceQueryService _sourceQueryService;
    private readonly ISourceCommandService _sourceCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private SourceListItem? _selectedSource;
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
    /// Initializes a new instance of the <see cref="SourcesViewModel"/> class.
    /// </summary>
    /// <param name="sourceQueryService">The source query service.</param>
    /// <param name="sourceCommandService">The source command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public SourcesViewModel(ISourceQueryService sourceQueryService, ISourceCommandService sourceCommandService, INavigationService navigationService)
    {
        _sourceQueryService = sourceQueryService ?? throw new ArgumentNullException(nameof(sourceQueryService));
        _sourceCommandService = sourceCommandService ?? throw new ArgumentNullException(nameof(sourceCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Sources";
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

        await LoadSourcesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadSourcesAsync(cancellationToken);
        GoBackCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanGoBack));
    }

    /// <summary>
    /// Gets the collection of sources to display.
    /// </summary>
    public ObservableCollection<SourceListItem> Sources { get; } = new();

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
    /// Gets or sets the selected source.
    /// </summary>
    public SourceListItem? SelectedSource
    {
        get => _selectedSource;
        set
        {
            if (SetProperty(ref _selectedSource, value))
            {
                EditSourceCommand.NotifyCanExecuteChanged();
                ActivateSourceCommand.NotifyCanExecuteChanged();
                DeactivateSourceCommand.NotifyCanExecuteChanged();
                DeleteSourceCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the source list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadSourcesAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Source screen.
    /// </summary>
    [RelayCommand]
    private async Task AddSourceAsync()
    {
        await _navigationService.NavigateAsync<AddSourceViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Source screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceCommand))]
    private async Task EditSourceAsync()
    {
        if (SelectedSource is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["SourceId"] = SelectedSource.Id
        };

        await _navigationService.NavigateAsync<EditSourceViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected source.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceCommand))]
    private async Task ActivateSourceAsync()
    {
        if (SelectedSource is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateSourceStatusRequest
        {
            SourceId = SelectedSource.Id,
            IsActive = true
        };

        SetBusy("Activating source...");
        var result = await _sourceCommandService.UpdateSourceStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleSourceStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate source.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected source.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceCommand))]
    private async Task DeactivateSourceAsync()
    {
        if (SelectedSource is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateSourceStatusRequest
        {
            SourceId = SelectedSource.Id,
            IsActive = false
        };

        SetBusy("Deactivating source...");
        var result = await _sourceCommandService.UpdateSourceStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleSourceStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate source.";
        }
    }

    private async Task HandleSourceStatusUpdateSuccess()
    {
        await LoadSourcesAsync();
        SelectedSource = null;
        ActivateSourceCommand.NotifyCanExecuteChanged();
        DeactivateSourceCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected source.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSourceCommand))]
    private async Task DeleteSourceAsync()
    {
        if (SelectedSource is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this source?\n\nThis action hides the source from the application.",
            "Delete Source",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteSourceRequest
        {
            SourceId = SelectedSource.Id
        };

        SetBusy("Deleting source...");
        var result = await _sourceCommandService.DeleteSourceAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadSourcesAsync();
            SelectedSource = null;
            DeleteSourceCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete source.";
        }
    }

    private bool CanExecuteSourceCommand()
    {
        return SelectedSource is not null;
    }

    /// <summary>
    /// Loads all sources.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadSourcesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading sources...");
        var sources = await _sourceQueryService.GetAllSourcesAsync(cancellationToken);
        UpdateSources(sources);
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
            await SearchSourcesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches sources based on the current search text.
    /// </summary>
    private async Task SearchSourcesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching sources...");
        var sources = await _sourceQueryService.SearchSourcesAsync(SearchText, cancellationToken);
        UpdateSources(sources);
        ClearBusy();
    }

    /// <summary>
    /// Updates the sources collection on the UI thread.
    /// </summary>
    /// <param name="sources">The sources to update.</param>
    private void UpdateSources(IEnumerable<SourceListItem> sources)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateSourcesInternal(sources);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateSourcesInternal(sources));
        }
    }

    /// <summary>
    /// Updates the sources collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="sources">The sources to update.</param>
    private void UpdateSourcesInternal(IEnumerable<SourceListItem> sources)
    {
        Sources.Clear();
        foreach (var source in sources)
        {
            Sources.Add(source);
        }
    }
}
