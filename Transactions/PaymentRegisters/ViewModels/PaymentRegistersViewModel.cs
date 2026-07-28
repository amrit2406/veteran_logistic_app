using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.PaymentRegisters.ViewModels;

/// <summary>
/// ViewModel for the Payment Registers listing screen.
/// </summary>
public sealed partial class PaymentRegistersViewModel : ViewModelBase
{
    private readonly IPaymentRegisterQueryService _paymentRegisterQueryService;
    private readonly IPaymentRegisterCommandService _paymentRegisterCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private PaymentRegisterListItem? _selectedPaymentRegister;
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
    /// Initializes a new instance of the <see cref="PaymentRegistersViewModel"/> class.
    /// </summary>
    /// <param name="paymentRegisterQueryService">The payment register query service.</param>
    /// <param name="paymentRegisterCommandService">The payment register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public PaymentRegistersViewModel(IPaymentRegisterQueryService paymentRegisterQueryService, IPaymentRegisterCommandService paymentRegisterCommandService, INavigationService navigationService)
    {
        _paymentRegisterQueryService = paymentRegisterQueryService ?? throw new ArgumentNullException(nameof(paymentRegisterQueryService));
        _paymentRegisterCommandService = paymentRegisterCommandService ?? throw new ArgumentNullException(nameof(paymentRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Payment Registers";
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

        await LoadPaymentRegistersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadPaymentRegistersAsync(cancellationToken);
        
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
    /// Gets the collection of payment registers to display.
    /// </summary>
    public ObservableCollection<PaymentRegisterListItem> PaymentRegisters { get; } = new();

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
    /// Gets or sets the selected payment register.
    /// </summary>
    public PaymentRegisterListItem? SelectedPaymentRegister
    {
        get => _selectedPaymentRegister;
        set
        {
            if (SetProperty(ref _selectedPaymentRegister, value))
            {
                EditPaymentRegisterCommand.NotifyCanExecuteChanged();
                ActivatePaymentRegisterCommand.NotifyCanExecuteChanged();
                DeactivatePaymentRegisterCommand.NotifyCanExecuteChanged();
                DeletePaymentRegisterCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the payment register list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadPaymentRegistersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Payment Register screen.
    /// </summary>
    [RelayCommand]
    private async Task AddPaymentRegisterAsync()
    {
        await _navigationService.NavigateAsync<AddPaymentRegisterViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Payment Register screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentRegisterCommand))]
    private async Task EditPaymentRegisterAsync()
    {
        if (SelectedPaymentRegister is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["PaymentRegisterId"] = SelectedPaymentRegister.Id
        };

        await _navigationService.NavigateAsync<EditPaymentRegisterViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected payment register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentRegisterCommand))]
    private async Task ActivatePaymentRegisterAsync()
    {
        if (SelectedPaymentRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePaymentRegisterStatusRequest
        {
            PaymentRegisterId = SelectedPaymentRegister.Id,
            IsActive = true
        };

        SetBusy("Activating payment register...");
        var result = await _paymentRegisterCommandService.UpdatePaymentRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePaymentRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate payment register.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected payment register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentRegisterCommand))]
    private async Task DeactivatePaymentRegisterAsync()
    {
        if (SelectedPaymentRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePaymentRegisterStatusRequest
        {
            PaymentRegisterId = SelectedPaymentRegister.Id,
            IsActive = false
        };

        SetBusy("Deactivating payment register...");
        var result = await _paymentRegisterCommandService.UpdatePaymentRegisterStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePaymentRegisterStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate payment register.";
        }
    }

    private async Task HandlePaymentRegisterStatusUpdateSuccess()
    {
        await LoadPaymentRegistersAsync();
        SelectedPaymentRegister = null;
        ActivatePaymentRegisterCommand.NotifyCanExecuteChanged();
        DeactivatePaymentRegisterCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected payment register.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentRegisterCommand))]
    private async Task DeletePaymentRegisterAsync()
    {
        if (SelectedPaymentRegister is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this payment register?\n\nThis action hides the payment register from the application.",
            "Delete Payment Register",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeletePaymentRegisterRequest
        {
            PaymentRegisterId = SelectedPaymentRegister.Id
        };

        SetBusy("Deleting payment register...");
        var result = await _paymentRegisterCommandService.DeletePaymentRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadPaymentRegistersAsync();
            SelectedPaymentRegister = null;
            DeletePaymentRegisterCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete payment register.";
        }
    }

    private bool CanExecutePaymentRegisterCommand()
    {
        return SelectedPaymentRegister is not null;
    }

    /// <summary>
    /// Loads all payment registers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPaymentRegistersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading payment registers...");
        var paymentRegisters = await _paymentRegisterQueryService.GetAllPaymentRegistersAsync(cancellationToken);
        UpdatePaymentRegisters(paymentRegisters);
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
            await SearchPaymentRegistersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches payment registers based on the current search text.
    /// </summary>
    private async Task SearchPaymentRegistersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching payment registers...");
        var paymentRegisters = await _paymentRegisterQueryService.SearchPaymentRegistersAsync(SearchText, cancellationToken);
        UpdatePaymentRegisters(paymentRegisters);
        ClearBusy();
    }

    /// <summary>
    /// Updates the payment registers collection on the UI thread.
    /// </summary>
    /// <param name="paymentRegisters">The payment registers to update.</param>
    private void UpdatePaymentRegisters(IEnumerable<PaymentRegisterListItem> paymentRegisters)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdatePaymentRegistersInternal(paymentRegisters);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdatePaymentRegistersInternal(paymentRegisters));
        }
    }

    /// <summary>
    /// Updates the payment registers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="paymentRegisters">The payment registers to update.</param>
    private void UpdatePaymentRegistersInternal(IEnumerable<PaymentRegisterListItem> paymentRegisters)
    {
        PaymentRegisters.Clear();
        foreach (var paymentRegister in paymentRegisters)
        {
            PaymentRegisters.Add(paymentRegister);
        }
    }
}
