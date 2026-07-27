using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.UnloadingRegisters.ViewModels;

/// <summary>
/// ViewModel for the Unloading Registers listing screen.
/// </summary>
public sealed partial class UnloadingRegistersViewModel : ViewModelBase
{
    private readonly IUnloadingRegisterQueryService _unloadingRegisterQueryService;
    private readonly IUnloadingRegisterCommandService _unloadingRegisterCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private UnloadingRegisterListItem? _selectedUnloadingRegister;
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
    /// Initializes a new instance of the <see cref="UnloadingRegistersViewModel"/> class.
    /// </summary>
    /// <param name="unloadingRegisterQueryService">The unloading register query service.</param>
    /// <param name="unloadingRegisterCommandService">The unloading register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public UnloadingRegistersViewModel(IUnloadingRegisterQueryService unloadingRegisterQueryService, IUnloadingRegisterCommandService unloadingRegisterCommandService, INavigationService navigationService)
    {
        _unloadingRegisterQueryService = unloadingRegisterQueryService ?? throw new ArgumentNullException(nameof(unloadingRegisterQueryService));
        _unloadingRegisterCommandService = unloadingRegisterCommandService ?? throw new ArgumentNullException(nameof(unloadingRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Unloading Registers";
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

        await LoadUnloadingRegistersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadUnloadingRegistersAsync(cancellationToken);
        
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
    /// Gets the collection of unloading registers to display.
    /// </summary>
    public ObservableCollection<UnloadingRegisterListItem> UnloadingRegisters { get; } = new();

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
    /// Gets or sets the selected unloading register.
    /// </summary>
    public UnloadingRegisterListItem? SelectedUnloadingRegister
    {
        get => _selectedUnloadingRegister;
        set
        {
            if (SetProperty(ref _selectedUnloadingRegister, value))
            {
                EditUnloadingRegisterCommand.NotifyCanExecuteChanged();
                ActivateUnloadingRegisterCommand.NotifyCanExecuteChanged();
                DeactivateUnloadingRegisterCommand.NotifyCanExecuteChanged();
                DeleteUnloadingRegisterCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the unloading register list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadUnloadingRegistersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Unloading Register screen.
    /// </summary>
    [RelayCommand]
    private async Task AddUnloadingRegisterAsync()
    {
        await _navigationService.NavigateAsync<AddUnloadingRegisterViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Unloading Register screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteUnloadingRegisterCommand))]
    private async Task EditUnloadingRegisterAsync()
    {
        if (SelectedUnloadingRegister is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["UnloadingRegisterId"] = SelectedUnloadingRegister.Id
        };

        await _navigationService.NavigateAsync<EditUnloadingRegisterViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected unloading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteUnloadingRegisterCommand))]
    private async Task ActivateUnloadingRegisterAsync()
    {
        if (SelectedUnloadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateUnloadingRegisterStatusRequest
        {
            UnloadingRegisterId = SelectedUnloadingRegister.Id,
            IsActive = true
        };

        SetBusy("Activating unloading register...");
        var result = await _unloadingRegisterCommandService.UpdateUnloadingRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleUnloadingRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate unloading register.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected unloading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteUnloadingRegisterCommand))]
    private async Task DeactivateUnloadingRegisterAsync()
    {
        if (SelectedUnloadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateUnloadingRegisterStatusRequest
        {
            UnloadingRegisterId = SelectedUnloadingRegister.Id,
            IsActive = false
        };

        SetBusy("Deactivating unloading register...");
        var result = await _unloadingRegisterCommandService.UpdateUnloadingRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleUnloadingRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate unloading register.";
        }
    }

    private async Task HandleUnloadingRegisterStatusUpdateSuccess()
    {
        await LoadUnloadingRegistersAsync();
        SelectedUnloadingRegister = null;
        ActivateUnloadingRegisterCommand.NotifyCanExecuteChanged();
        DeactivateUnloadingRegisterCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected unloading register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteUnloadingRegisterCommand))]
    private async Task DeleteUnloadingRegisterAsync()
    {
        if (SelectedUnloadingRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this unloading register?\n\nThis action hides the unloading register from the application.",
            "Delete Unloading Register",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteUnloadingRegisterRequest
        {
            UnloadingRegisterId = SelectedUnloadingRegister.Id
        };

        SetBusy("Deleting unloading register...");
        var result = await _unloadingRegisterCommandService.DeleteUnloadingRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadUnloadingRegistersAsync();
            SelectedUnloadingRegister = null;
            DeleteUnloadingRegisterCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete unloading register.";
        }
    }

    private bool CanExecuteUnloadingRegisterCommand()
    {
        return SelectedUnloadingRegister is not null;
    }

    /// <summary>
    /// Loads all unloading registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadUnloadingRegistersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading unloading registers...");
        var unloadingRegisters = await _unloadingRegisterQueryService.GetAllUnloadingRegistersAsync(cancellationToken);
        UpdateUnloadingRegisters(unloadingRegisters);
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
            await SearchUnloadingRegistersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches unloading registers based on the current search text.
    /// </summary>
    private async Task SearchUnloadingRegistersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching unloading registers...");
        var unloadingRegisters = await _unloadingRegisterQueryService.SearchUnloadingRegistersAsync(SearchText, cancellationToken);
        UpdateUnloadingRegisters(unloadingRegisters);
        ClearBusy();
    }

    /// <summary>
    /// Updates the unloading registers collection on the UI thread.
    /// </summary>
    /// <param name="unloadingRegisters">The unloading registers to update.</param>
    private void UpdateUnloadingRegisters(IEnumerable<UnloadingRegisterListItem> unloadingRegisters)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateUnloadingRegistersInternal(unloadingRegisters);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateUnloadingRegistersInternal(unloadingRegisters));
        }
    }

    /// <summary>
    /// Updates the unloading registers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="unloadingRegisters">The unloading registers to update.</param>
    private void UpdateUnloadingRegistersInternal(IEnumerable<UnloadingRegisterListItem> unloadingRegisters)
    {
        UnloadingRegisters.Clear();
        foreach (var unloadingRegister in unloadingRegisters)
        {
            UnloadingRegisters.Add(unloadingRegister);
        }
    }
}
