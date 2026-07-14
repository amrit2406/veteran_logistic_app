using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.HsdRates.ViewModels;

/// <summary>
/// ViewModel for the HSD Rates listing screen.
/// </summary>
public sealed partial class HsdRatesViewModel : ViewModelBase
{
    private readonly IHsdRateQueryService _hsdRateQueryService;
    private readonly IHsdRateCommandService _hsdRateCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private HsdRateListItem? _selectedHsdRate;
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
    /// Initializes a new instance of the <see cref="HsdRatesViewModel"/> class.
    /// </summary>
    /// <param name="hsdRateQueryService">The HSD rate query service.</param>
    /// <param name="hsdRateCommandService">The HSD rate command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public HsdRatesViewModel(IHsdRateQueryService hsdRateQueryService, IHsdRateCommandService hsdRateCommandService, INavigationService navigationService)
    {
        _hsdRateQueryService = hsdRateQueryService ?? throw new ArgumentNullException(nameof(hsdRateQueryService));
        _hsdRateCommandService = hsdRateCommandService ?? throw new ArgumentNullException(nameof(hsdRateCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "HSD Rates";
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

        await LoadHsdRatesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadHsdRatesAsync(cancellationToken);
        
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
    /// Gets the collection of HSD rates to display.
    /// </summary>
    public ObservableCollection<HsdRateListItem> HsdRates { get; } = new();

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
    /// Gets or sets the selected HSD rate.
    /// </summary>
    public HsdRateListItem? SelectedHsdRate
    {
        get => _selectedHsdRate;
        set
        {
            if (SetProperty(ref _selectedHsdRate, value))
            {
                EditHsdRateCommand.NotifyCanExecuteChanged();
                ActivateHsdRateCommand.NotifyCanExecuteChanged();
                DeactivateHsdRateCommand.NotifyCanExecuteChanged();
                DeleteHsdRateCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the HSD rate list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadHsdRatesAsync();
    }

    /// <summary>
    /// Command to navigate to the Add HSD Rate screen.
    /// </summary>
    [RelayCommand]
    private async Task AddHsdRateAsync()
    {
        await _navigationService.NavigateAsync<AddHsdRateViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit HSD Rate screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteHsdRateCommand))]
    private async Task EditHsdRateAsync()
    {
        if (SelectedHsdRate is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["HsdRateId"] = SelectedHsdRate.Id
        };

        await _navigationService.NavigateAsync<EditHsdRateViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected HSD rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteHsdRateCommand))]
    private async Task ActivateHsdRateAsync()
    {
        if (SelectedHsdRate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateHsdRateStatusRequest
        {
            HsdRateId = SelectedHsdRate.Id,
            IsActive = true
        };

        SetBusy("Activating HSD rate...");
        var result = await _hsdRateCommandService.UpdateHsdRateStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleHsdRateStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate HSD rate.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected HSD rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteHsdRateCommand))]
    private async Task DeactivateHsdRateAsync()
    {
        if (SelectedHsdRate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateHsdRateStatusRequest
        {
            HsdRateId = SelectedHsdRate.Id,
            IsActive = false
        };

        SetBusy("Deactivating HSD rate...");
        var result = await _hsdRateCommandService.UpdateHsdRateStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleHsdRateStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate HSD rate.";
        }
    }

    private async Task HandleHsdRateStatusUpdateSuccess()
    {
        await LoadHsdRatesAsync();
        SelectedHsdRate = null;
        ActivateHsdRateCommand.NotifyCanExecuteChanged();
        DeactivateHsdRateCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected HSD rate.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteHsdRateCommand))]
    private async Task DeleteHsdRateAsync()
    {
        if (SelectedHsdRate is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this HSD rate?\n\nThis action hides the HSD rate from the application.",
            "Delete HSD Rate",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteHsdRateRequest
        {
            HsdRateId = SelectedHsdRate.Id
        };

        SetBusy("Deleting HSD rate...");
        var result = await _hsdRateCommandService.DeleteHsdRateAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadHsdRatesAsync();
            SelectedHsdRate = null;
            DeleteHsdRateCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete HSD rate.";
        }
    }

    private bool CanExecuteHsdRateCommand()
    {
        return SelectedHsdRate is not null;
    }

    /// <summary>
    /// Loads all HSD rates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadHsdRatesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading HSD rates...");
        var hsdRates = await _hsdRateQueryService.GetAllHsdRatesAsync(cancellationToken);
        UpdateHsdRates(hsdRates);
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
            await SearchHsdRatesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches HSD rates based on the current search text.
    /// </summary>
    private async Task SearchHsdRatesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching HSD rates...");
        var hsdRates = await _hsdRateQueryService.SearchHsdRatesAsync(SearchText, cancellationToken);
        UpdateHsdRates(hsdRates);
        ClearBusy();
    }

    /// <summary>
    /// Updates the HSD rates collection on the UI thread.
    /// </summary>
    /// <param name="hsdRates">The HSD rates to update.</param>
    private void UpdateHsdRates(IReadOnlyList<HsdRateListItem> hsdRates)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateHsdRatesInternal(hsdRates);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateHsdRatesInternal(hsdRates));
        }
    }

    /// <summary>
    /// Updates the HSD rates collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="hsdRates">The HSD rates to update.</param>
    private void UpdateHsdRatesInternal(IReadOnlyList<HsdRateListItem> hsdRates)
    {
        HsdRates.Clear();
        foreach (var hsdRate in hsdRates)
        {
            HsdRates.Add(hsdRate);
        }
    }
}
