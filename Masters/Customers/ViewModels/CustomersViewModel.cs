using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Customers.ViewModels;

/// <summary>
/// ViewModel for the Customers listing screen.
/// </summary>
public sealed partial class CustomersViewModel : ViewModelBase
{
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ICustomerCommandService _customerCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private CustomerListItem? _selectedCustomer;
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
    /// Initializes a new instance of the <see cref="CustomersViewModel"/> class.
    /// </summary>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="customerCommandService">The customer command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public CustomersViewModel(ICustomerQueryService customerQueryService, ICustomerCommandService customerCommandService, INavigationService navigationService)
    {
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _customerCommandService = customerCommandService ?? throw new ArgumentNullException(nameof(customerCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Customers";
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

        await LoadCustomersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadCustomersAsync(cancellationToken);
        GoBackCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanGoBack));
    }

    /// <summary>
    /// Gets the collection of customers to display.
    /// </summary>
    public ObservableCollection<CustomerListItem> Customers { get; } = new();

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
    /// Gets or sets the selected customer.
    /// </summary>
    public CustomerListItem? SelectedCustomer
    {
        get => _selectedCustomer;
        set
        {
            if (SetProperty(ref _selectedCustomer, value))
            {
                EditCustomerCommand.NotifyCanExecuteChanged();
                ActivateCustomerCommand.NotifyCanExecuteChanged();
                DeactivateCustomerCommand.NotifyCanExecuteChanged();
                DeleteCustomerCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the customer list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadCustomersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Customer screen.
    /// </summary>
    [RelayCommand]
    private async Task AddCustomerAsync()
    {
        await _navigationService.NavigateAsync<AddCustomerViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Customer screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCustomerCommand))]
    private async Task EditCustomerAsync()
    {
        if (SelectedCustomer is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["CustomerId"] = SelectedCustomer.Id
        };

        await _navigationService.NavigateAsync<EditCustomerViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected customer.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCustomerCommand))]
    private async Task ActivateCustomerAsync()
    {
        if (SelectedCustomer is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateCustomerStatusRequest
        {
            CustomerId = SelectedCustomer.Id,
            IsActive = true
        };

        SetBusy("Activating customer...");
        var result = await _customerCommandService.UpdateCustomerStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleCustomerStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate customer.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected customer.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCustomerCommand))]
    private async Task DeactivateCustomerAsync()
    {
        if (SelectedCustomer is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateCustomerStatusRequest
        {
            CustomerId = SelectedCustomer.Id,
            IsActive = false
        };

        SetBusy("Deactivating customer...");
        var result = await _customerCommandService.UpdateCustomerStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleCustomerStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate customer.";
        }
    }

    private async Task HandleCustomerStatusUpdateSuccess()
    {
        await LoadCustomersAsync();
        SelectedCustomer = null;
        ActivateCustomerCommand.NotifyCanExecuteChanged();
        DeactivateCustomerCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected customer.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCustomerCommand))]
    private async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this customer?\n\nThis action hides the customer from the application.",
            "Delete Customer",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteCustomerRequest
        {
            CustomerId = SelectedCustomer.Id
        };

        SetBusy("Deleting customer...");
        var result = await _customerCommandService.DeleteCustomerAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadCustomersAsync();
            SelectedCustomer = null;
            DeleteCustomerCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete customer.";
        }
    }

    private bool CanExecuteCustomerCommand()
    {
        return SelectedCustomer is not null;
    }

    /// <summary>
    /// Loads all customers.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadCustomersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading customers...");
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
        UpdateCustomers(customers);
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
            await SearchCustomersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches customers based on the current search text.
    /// </summary>
    private async Task SearchCustomersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching customers...");
        var customers = await _customerQueryService.SearchCustomersAsync(SearchText, cancellationToken);
        UpdateCustomers(customers);
        ClearBusy();
    }

    /// <summary>
    /// Updates the customers collection on the UI thread.
    /// </summary>
    /// <param name="customers">The customers to update.</param>
    private void UpdateCustomers(IEnumerable<CustomerListItem> customers)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateCustomersInternal(customers);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateCustomersInternal(customers));
        }
    }

    /// <summary>
    /// Updates the customers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="customers">The customers to update.</param>
    private void UpdateCustomersInternal(IEnumerable<CustomerListItem> customers)
    {
        Customers.Clear();
        foreach (var customer in customers)
        {
            Customers.Add(customer);
        }
    }
}
