using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.PartyBillRegister.ViewModels;

/// <summary>
/// ViewModel for the Party Bill Registers listing screen.
/// </summary>
public sealed partial class PartyBillRegistersViewModel : ViewModelBase
{
    private readonly IPartyBillRegisterQueryService _partyBillRegisterQueryService;
    private readonly IPartyBillRegisterCommandService _partyBillRegisterCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private PartyBillRegisterListItem? _selectedPartyBillRegister;
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
    /// Initializes a new instance of the <see cref="PartyBillRegistersViewModel"/> class.
    /// </summary>
    /// <param name="partyBillRegisterQueryService">The party bill register query service.</param>
    /// <param name="partyBillRegisterCommandService">The party bill register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public PartyBillRegistersViewModel(IPartyBillRegisterQueryService partyBillRegisterQueryService, IPartyBillRegisterCommandService partyBillRegisterCommandService, INavigationService navigationService)
    {
        _partyBillRegisterQueryService = partyBillRegisterQueryService ?? throw new ArgumentNullException(nameof(partyBillRegisterQueryService));
        _partyBillRegisterCommandService = partyBillRegisterCommandService ?? throw new ArgumentNullException(nameof(partyBillRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Party Bill Registers";
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

        await LoadPartyBillRegistersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadPartyBillRegistersAsync(cancellationToken);
        
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
    /// Gets the collection of party bill registers to display.
    /// </summary>
    public ObservableCollection<PartyBillRegisterListItem> PartyBillRegisters { get; } = new();

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
    /// Gets or sets the selected party bill register.
    /// </summary>
    public PartyBillRegisterListItem? SelectedPartyBillRegister
    {
        get => _selectedPartyBillRegister;
        set
        {
            if (SetProperty(ref _selectedPartyBillRegister, value))
            {
                EditPartyBillRegisterCommand.NotifyCanExecuteChanged();
                ActivatePartyBillRegisterCommand.NotifyCanExecuteChanged();
                DeactivatePartyBillRegisterCommand.NotifyCanExecuteChanged();
                DeletePartyBillRegisterCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the party bill register list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadPartyBillRegistersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Party Bill Register screen.
    /// </summary>
    [RelayCommand]
    private async Task AddPartyBillRegisterAsync()
    {
        await _navigationService.NavigateAsync<AddPartyBillRegisterViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Party Bill Register screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePartyBillRegisterCommand))]
    private async Task EditPartyBillRegisterAsync()
    {
        if (SelectedPartyBillRegister is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["PartyBillRegisterId"] = SelectedPartyBillRegister.Id
        };

        await _navigationService.NavigateAsync<EditPartyBillRegisterViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected party bill register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePartyBillRegisterCommand))]
    private async Task ActivatePartyBillRegisterAsync()
    {
        if (SelectedPartyBillRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePartyBillRegisterStatusRequest
        {
            PartyBillRegisterId = SelectedPartyBillRegister.Id,
            IsActive = true,
            ModifiedBy = "System"
        };

        SetBusy("Activating party bill register...");
        var result = await _partyBillRegisterCommandService.UpdatePartyBillRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePartyBillRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate party bill register.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected party bill register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePartyBillRegisterCommand))]
    private async Task DeactivatePartyBillRegisterAsync()
    {
        if (SelectedPartyBillRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePartyBillRegisterStatusRequest
        {
            PartyBillRegisterId = SelectedPartyBillRegister.Id,
            IsActive = false,
            ModifiedBy = "System"
        };

        SetBusy("Deactivating party bill register...");
        var result = await _partyBillRegisterCommandService.UpdatePartyBillRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePartyBillRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate party bill register.";
        }
    }

    private async Task HandlePartyBillRegisterStatusUpdateSuccess()
    {
        await LoadPartyBillRegistersAsync();
        SelectedPartyBillRegister = null;
        ActivatePartyBillRegisterCommand.NotifyCanExecuteChanged();
        DeactivatePartyBillRegisterCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected party bill register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePartyBillRegisterCommand))]
    private async Task DeletePartyBillRegisterAsync()
    {
        if (SelectedPartyBillRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this party bill register?\n\nThis action hides the party bill register from the application.",
            "Delete Party Bill Register",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeletePartyBillRegisterRequest
        {
            PartyBillRegisterId = SelectedPartyBillRegister.Id,
            DeletedBy = "System"
        };

        SetBusy("Deleting party bill register...");
        var result = await _partyBillRegisterCommandService.DeletePartyBillRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadPartyBillRegistersAsync();
            SelectedPartyBillRegister = null;
            DeletePartyBillRegisterCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete party bill register.";
        }
    }

    private bool CanExecutePartyBillRegisterCommand()
    {
        return SelectedPartyBillRegister is not null;
    }

    /// <summary>
    /// Loads all party bill registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPartyBillRegistersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading party bill registers...");
        var partyBillRegisters = await _partyBillRegisterQueryService.GetAllPartyBillRegistersAsync(cancellationToken);
        UpdatePartyBillRegisters(partyBillRegisters);
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
            await SearchPartyBillRegistersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches party bill registers based on the current search text.
    /// </summary>
    private async Task SearchPartyBillRegistersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching party bill registers...");
        var partyBillRegisters = await _partyBillRegisterQueryService.SearchPartyBillRegistersAsync(SearchText, cancellationToken);
        UpdatePartyBillRegisters(partyBillRegisters);
        ClearBusy();
    }

    /// <summary>
    /// Updates the party bill registers collection on the UI thread.
    /// </summary>
    /// <param name="partyBillRegisters">The party bill registers to update.</param>
    private void UpdatePartyBillRegisters(IEnumerable<PartyBillRegisterListItem> partyBillRegisters)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdatePartyBillRegistersInternal(partyBillRegisters);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdatePartyBillRegistersInternal(partyBillRegisters));
        }
    }

    /// <summary>
    /// Updates the party bill registers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="partyBillRegisters">The party bill registers to update.</param>
    private void UpdatePartyBillRegistersInternal(IEnumerable<PartyBillRegisterListItem> partyBillRegisters)
    {
        PartyBillRegisters.Clear();
        foreach (var partyBillRegister in partyBillRegisters)
        {
            PartyBillRegisters.Add(partyBillRegister);
        }
    }
}
