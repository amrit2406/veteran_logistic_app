using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.DORates.ViewModels;

/// <summary>
/// ViewModel for the DO Rates listing screen.
/// </summary>
public sealed partial class DORatesViewModel : ViewModelBase
{
    private readonly IDORateQueryService _doRateQueryService;
    private readonly IDORateCommandService _doRateCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private DORateListItem? _selectedDORate;
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
    /// Initializes a new instance of the <see cref="DORatesViewModel"/> class.
    /// </summary>
    /// <param name="doRateQueryService">The DO rate query service.</param>
    /// <param name="doRateCommandService">The DO rate command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public DORatesViewModel(IDORateQueryService doRateQueryService, IDORateCommandService doRateCommandService, INavigationService navigationService)
    {
        _doRateQueryService = doRateQueryService ?? throw new ArgumentNullException(nameof(doRateQueryService));
        _doRateCommandService = doRateCommandService ?? throw new ArgumentNullException(nameof(doRateCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "DO Rates";
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

        await LoadDORatesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadDORatesAsync(cancellationToken);
        
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
    /// Gets the collection of DO rates to display.
    /// </summary>
    public ObservableCollection<DORateListItem> DORates { get; } = new();

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
    /// Gets or sets the selected DO rate.
    /// </summary>
    public DORateListItem? SelectedDORate
    {
        get => _selectedDORate;
        set
        {
            if (SetProperty(ref _selectedDORate, value))
            {
                EditDORateCommand.NotifyCanExecuteChanged();
                ActivateDORateCommand.NotifyCanExecuteChanged();
                DeactivateDORateCommand.NotifyCanExecuteChanged();
                DeleteDORateCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the DO rate list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDORatesAsync();
    }

    /// <summary>
    /// Command to navigate to the Add DO Rate screen.
    /// </summary>
    [RelayCommand]
    private async Task AddDORateAsync()
    {
        await _navigationService.NavigateAsync<AddDORateViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit DO Rate screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDORateCommand))]
    private async Task EditDORateAsync()
    {
        if (SelectedDORate is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["DORateId"] = SelectedDORate.Id
        };

        await _navigationService.NavigateAsync<EditDORateViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected DO rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDORateCommand))]
    private async Task ActivateDORateAsync()
    {
        if (SelectedDORate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateDORateStatusRequest
        {
            DORateId = SelectedDORate.Id,
            IsActive = true
        };

        SetBusy("Activating DO rate...");
        var result = await _doRateCommandService.UpdateDORateStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleDORateStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate DO rate.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected DO rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDORateCommand))]
    private async Task DeactivateDORateAsync()
    {
        if (SelectedDORate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateDORateStatusRequest
        {
            DORateId = SelectedDORate.Id,
            IsActive = false
        };

        SetBusy("Deactivating DO rate...");
        var result = await _doRateCommandService.UpdateDORateStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleDORateStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate DO rate.";
        }
    }

    private async Task HandleDORateStatusUpdateSuccess()
    {
        await LoadDORatesAsync();
        SelectedDORate = null;
        ActivateDORateCommand.NotifyCanExecuteChanged();
        DeactivateDORateCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected DO rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDORateCommand))]
    private async Task DeleteDORateAsync()
    {
        if (SelectedDORate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this DO rate?\n\nThis action hides the DO rate from the application.",
            "Delete DO Rate",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteDORateRequest
        {
            DORateId = SelectedDORate.Id
        };

        SetBusy("Deleting DO rate...");
        var result = await _doRateCommandService.DeleteDORateAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadDORatesAsync();
            SelectedDORate = null;
            DeleteDORateCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete DO rate.";
        }
    }

    private bool CanExecuteDORateCommand()
    {
        return SelectedDORate is not null;
    }

    /// <summary>
    /// Loads all DO rates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadDORatesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading DO rates...");
        var doRates = await _doRateQueryService.GetAllDORatesAsync(cancellationToken);
        UpdateDORates(doRates);
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
            await SearchDORatesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches DO rates based on the current search text.
    /// </summary>
    private async Task SearchDORatesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching DO rates...");
        var doRates = await _doRateQueryService.SearchDORatesAsync(SearchText, cancellationToken);
        UpdateDORates(doRates);
        ClearBusy();
    }

    /// <summary>
    /// Updates the DO rates collection on the UI thread.
    /// </summary>
    /// <param name="doRates">The DO rates to update.</param>
    private void UpdateDORates(IEnumerable<DORateListItem> doRates)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateDORatesInternal(doRates);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateDORatesInternal(doRates));
        }
    }

    /// <summary>
    /// Updates the DO rates collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="doRates">The DO rates to update.</param>
    private void UpdateDORatesInternal(IEnumerable<DORateListItem> doRates)
    {
        DORates.Clear();
        foreach (var doRate in doRates)
        {
            DORates.Add(doRate);
        }
    }
}
