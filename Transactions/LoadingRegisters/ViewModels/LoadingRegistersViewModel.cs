using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.LoadingRegisters.ViewModels;

/// <summary>
/// ViewModel for the Loading Registers listing screen.
/// </summary>
public sealed partial class LoadingRegistersViewModel : ViewModelBase
{
    private readonly ILoadingRegisterQueryService _loadingRegisterQueryService;
    private readonly ILoadingRegisterCommandService _loadingRegisterCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private LoadingRegisterListItem? _selectedLoadingRegister;
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
    /// Initializes a new instance of the <see cref="LoadingRegistersViewModel"/> class.
    /// </summary>
    /// <param name="loadingRegisterQueryService">The loading register query service.</param>
    /// <param name="loadingRegisterCommandService">The loading register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public LoadingRegistersViewModel(ILoadingRegisterQueryService loadingRegisterQueryService, ILoadingRegisterCommandService loadingRegisterCommandService, INavigationService navigationService)
    {
        _loadingRegisterQueryService = loadingRegisterQueryService ?? throw new ArgumentNullException(nameof(loadingRegisterQueryService));
        _loadingRegisterCommandService = loadingRegisterCommandService ?? throw new ArgumentNullException(nameof(loadingRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Loading Registers";
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

        await LoadLoadingRegistersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadLoadingRegistersAsync(cancellationToken);
        
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
    /// Gets the collection of loading registers to display.
    /// </summary>
    public ObservableCollection<LoadingRegisterListItem> LoadingRegisters { get; } = new();

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
    /// Gets or sets the selected loading register.
    /// </summary>
    public LoadingRegisterListItem? SelectedLoadingRegister
    {
        get => _selectedLoadingRegister;
        set
        {
            if (SetProperty(ref _selectedLoadingRegister, value))
            {
                EditLoadingRegisterCommand.NotifyCanExecuteChanged();
                ActivateLoadingRegisterCommand.NotifyCanExecuteChanged();
                DeactivateLoadingRegisterCommand.NotifyCanExecuteChanged();
                DeleteLoadingRegisterCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the loading register list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadLoadingRegistersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Loading Register screen.
    /// </summary>
    [RelayCommand]
    private async Task AddLoadingRegisterAsync()
    {
        await _navigationService.NavigateAsync<AddLoadingRegisterViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Loading Register screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteLoadingRegisterCommand))]
    private async Task EditLoadingRegisterAsync()
    {
        if (SelectedLoadingRegister is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["LoadingRegisterId"] = SelectedLoadingRegister.Id
        };

        await _navigationService.NavigateAsync<EditLoadingRegisterViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected loading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteLoadingRegisterCommand))]
    private async Task ActivateLoadingRegisterAsync()
    {
        if (SelectedLoadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateLoadingRegisterStatusRequest
        {
            LoadingRegisterId = SelectedLoadingRegister.Id,
            IsActive = true
        };

        SetBusy("Activating loading register...");
        var result = await _loadingRegisterCommandService.UpdateLoadingRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleLoadingRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate loading register.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected loading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteLoadingRegisterCommand))]
    private async Task DeactivateLoadingRegisterAsync()
    {
        if (SelectedLoadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateLoadingRegisterStatusRequest
        {
            LoadingRegisterId = SelectedLoadingRegister.Id,
            IsActive = false
        };

        SetBusy("Deactivating loading register...");
        var result = await _loadingRegisterCommandService.UpdateLoadingRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleLoadingRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate loading register.";
        }
    }

    private async Task HandleLoadingRegisterStatusUpdateSuccess()
    {
        await LoadLoadingRegistersAsync();
        SelectedLoadingRegister = null;
        ActivateLoadingRegisterCommand.NotifyCanExecuteChanged();
        DeactivateLoadingRegisterCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected loading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteLoadingRegisterCommand))]
    private async Task DeleteLoadingRegisterAsync()
    {
        if (SelectedLoadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this loading register?\n\nThis action hides the loading register from the application.",
            "Delete Loading Register",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteLoadingRegisterRequest
        {
            LoadingRegisterId = SelectedLoadingRegister.Id
        };

        SetBusy("Deleting loading register...");
        var result = await _loadingRegisterCommandService.DeleteLoadingRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadLoadingRegistersAsync();
            SelectedLoadingRegister = null;
            DeleteLoadingRegisterCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete loading register.";
        }
    }

    private bool CanExecuteLoadingRegisterCommand()
    {
        return SelectedLoadingRegister is not null;
    }

    /// <summary>
    /// Loads all loading registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadLoadingRegistersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading loading registers...");
        var loadingRegisters = await _loadingRegisterQueryService.GetAllLoadingRegistersAsync(cancellationToken);
        UpdateLoadingRegisters(loadingRegisters);
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
            await SearchLoadingRegistersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches loading registers based on the current search text.
    /// </summary>
    private async Task SearchLoadingRegistersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching loading registers...");
        var loadingRegisters = await _loadingRegisterQueryService.SearchLoadingRegistersAsync(SearchText, cancellationToken);
        UpdateLoadingRegisters(loadingRegisters);
        ClearBusy();
    }

    /// <summary>
    /// Updates the loading registers collection on the UI thread.
    /// </summary>
    /// <param name="loadingRegisters">The loading registers to update.</param>
    private void UpdateLoadingRegisters(IEnumerable<LoadingRegisterListItem> loadingRegisters)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateLoadingRegistersInternal(loadingRegisters);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateLoadingRegistersInternal(loadingRegisters));
        }
    }

    /// <summary>
    /// Updates the loading registers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="loadingRegisters">The loading registers to update.</param>
    private void UpdateLoadingRegistersInternal(IEnumerable<LoadingRegisterListItem> loadingRegisters)
    {
        LoadingRegisters.Clear();
        foreach (var loadingRegister in loadingRegisters)
        {
            LoadingRegisters.Add(loadingRegister);
        }
    }
}
