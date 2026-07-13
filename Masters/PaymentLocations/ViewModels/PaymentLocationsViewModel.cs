using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.PaymentLocations.ViewModels;

/// <summary>
/// ViewModel for the Payment Locations listing screen.
/// </summary>
public sealed partial class PaymentLocationsViewModel : ViewModelBase
{
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private readonly IPaymentLocationCommandService _paymentLocationCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private PaymentLocationListItem? _selectedPaymentLocation;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentLocationsViewModel"/> class.
    /// </summary>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    /// <param name="paymentLocationCommandService">The payment location command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public PaymentLocationsViewModel(IPaymentLocationQueryService paymentLocationQueryService, IPaymentLocationCommandService paymentLocationCommandService, INavigationService navigationService)
    {
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));
        _paymentLocationCommandService = paymentLocationCommandService ?? throw new ArgumentNullException(nameof(paymentLocationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Payment Locations";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadPaymentLocationsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadPaymentLocationsAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of payment locations to display.
    /// </summary>
    public ObservableCollection<PaymentLocationListItem> PaymentLocations { get; } = new();

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
    /// Gets or sets the selected payment location.
    /// </summary>
    public PaymentLocationListItem? SelectedPaymentLocation
    {
        get => _selectedPaymentLocation;
        set
        {
            if (SetProperty(ref _selectedPaymentLocation, value))
            {
                EditPaymentLocationCommand.NotifyCanExecuteChanged();
                ActivatePaymentLocationCommand.NotifyCanExecuteChanged();
                DeactivatePaymentLocationCommand.NotifyCanExecuteChanged();
                DeletePaymentLocationCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the payment location list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadPaymentLocationsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Payment Location screen.
    /// </summary>
    [RelayCommand]
    private async Task AddPaymentLocationAsync()
    {
        await _navigationService.NavigateAsync<AddPaymentLocationViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Payment Location screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentLocationCommand))]
    private async Task EditPaymentLocationAsync()
    {
        if (SelectedPaymentLocation is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["PaymentLocationId"] = SelectedPaymentLocation.Id
        };

        await _navigationService.NavigateAsync<EditPaymentLocationViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected payment location.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentLocationCommand))]
    private async Task ActivatePaymentLocationAsync()
    {
        if (SelectedPaymentLocation is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePaymentLocationStatusRequest
        {
            PaymentLocationId = SelectedPaymentLocation.Id,
            IsActive = true
        };

        SetBusy("Activating payment location...");
        var result = await _paymentLocationCommandService.UpdatePaymentLocationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePaymentLocationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate payment location.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected payment location.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentLocationCommand))]
    private async Task DeactivatePaymentLocationAsync()
    {
        if (SelectedPaymentLocation is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdatePaymentLocationStatusRequest
        {
            PaymentLocationId = SelectedPaymentLocation.Id,
            IsActive = false
        };

        SetBusy("Deactivating payment location...");
        var result = await _paymentLocationCommandService.UpdatePaymentLocationStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandlePaymentLocationStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate payment location.";
        }
    }

    private async Task HandlePaymentLocationStatusUpdateSuccess()
    {
        await LoadPaymentLocationsAsync();
        SelectedPaymentLocation = null;
        ActivatePaymentLocationCommand.NotifyCanExecuteChanged();
        DeactivatePaymentLocationCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected payment location.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecutePaymentLocationCommand))]
    private async Task DeletePaymentLocationAsync()
    {
        if (SelectedPaymentLocation is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this payment location?\n\nThis action hides the payment location from the application.",
            "Delete Payment Location",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeletePaymentLocationRequest
        {
            PaymentLocationId = SelectedPaymentLocation.Id
        };

        SetBusy("Deleting payment location...");
        var result = await _paymentLocationCommandService.DeletePaymentLocationAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadPaymentLocationsAsync();
            SelectedPaymentLocation = null;
            DeletePaymentLocationCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete payment location.";
        }
    }

    private bool CanExecutePaymentLocationCommand()
    {
        return SelectedPaymentLocation is not null;
    }

    /// <summary>
    /// Loads all payment locations.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPaymentLocationsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading payment locations...");
        var paymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync(cancellationToken);
        UpdatePaymentLocations(paymentLocations);
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
            await SearchPaymentLocationsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches payment locations based on the current search text.
    /// </summary>
    private async Task SearchPaymentLocationsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching payment locations...");
        var paymentLocations = await _paymentLocationQueryService.SearchPaymentLocationsAsync(SearchText, cancellationToken);
        UpdatePaymentLocations(paymentLocations);
        ClearBusy();
    }

    /// <summary>
    /// Updates the payment locations collection on the UI thread.
    /// </summary>
    /// <param name="paymentLocations">The payment locations to update.</param>
    private void UpdatePaymentLocations(IReadOnlyList<PaymentLocationListItem> paymentLocations)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdatePaymentLocationsInternal(paymentLocations);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdatePaymentLocationsInternal(paymentLocations));
        }
    }

    /// <summary>
    /// Updates the payment locations collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="paymentLocations">The payment locations to update.</param>
    private void UpdatePaymentLocationsInternal(IReadOnlyList<PaymentLocationListItem> paymentLocations)
    {
        PaymentLocations.Clear();
        foreach (var paymentLocation in paymentLocations)
        {
            PaymentLocations.Add(paymentLocation);
        }
    }
}
