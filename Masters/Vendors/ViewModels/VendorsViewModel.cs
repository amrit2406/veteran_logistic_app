using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Vendors.ViewModels;

/// <summary>
/// ViewModel for the Vendors listing screen.
/// </summary>
public sealed partial class VendorsViewModel : ViewModelBase
{
    private readonly IVendorQueryService _vendorQueryService;
    private readonly IVendorCommandService _vendorCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private VendorListItem? _selectedVendor;
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
    /// Initializes a new instance of the <see cref="VendorsViewModel"/> class.
    /// </summary>
    /// <param name="vendorQueryService">The vendor query service.</param>
    /// <param name="vendorCommandService">The vendor command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public VendorsViewModel(IVendorQueryService vendorQueryService, IVendorCommandService vendorCommandService, INavigationService navigationService)
    {
        _vendorQueryService = vendorQueryService ?? throw new ArgumentNullException(nameof(vendorQueryService));
        _vendorCommandService = vendorCommandService ?? throw new ArgumentNullException(nameof(vendorCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Vendors";
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

        await LoadVendorsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadVendorsAsync(cancellationToken);
        GoBackCommand.NotifyCanExecuteChanged();
        OnPropertyChanged(nameof(CanGoBack));
    }

    /// <summary>
    /// Gets the collection of vendors to display.
    /// </summary>
    public ObservableCollection<VendorListItem> Vendors { get; } = new();

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
    /// Gets or sets the selected vendor.
    /// </summary>
    public VendorListItem? SelectedVendor
    {
        get => _selectedVendor;
        set
        {
            if (SetProperty(ref _selectedVendor, value))
            {
                EditVendorCommand.NotifyCanExecuteChanged();
                ActivateVendorCommand.NotifyCanExecuteChanged();
                DeactivateVendorCommand.NotifyCanExecuteChanged();
                DeleteVendorCommand.NotifyCanExecuteChanged();
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
    /// Command to refresh the vendor list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadVendorsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Vendor screen.
    /// </summary>
    [RelayCommand]
    private async Task AddVendorAsync()
    {
        await _navigationService.NavigateAsync<AddVendorViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Vendor screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVendorCommand))]
    private async Task EditVendorAsync()
    {
        if (SelectedVendor is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["VendorId"] = SelectedVendor.Id
        };

        await _navigationService.NavigateAsync<EditVendorViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected vendor.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVendorCommand))]
    private async Task ActivateVendorAsync()
    {
        if (SelectedVendor is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVendorStatusRequest
        {
            VendorId = SelectedVendor.Id,
            IsActive = true
        };

        SetBusy("Activating vendor...");
        var result = await _vendorCommandService.UpdateVendorStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVendorStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate vendor.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected vendor.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVendorCommand))]
    private async Task DeactivateVendorAsync()
    {
        if (SelectedVendor is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateVendorStatusRequest
        {
            VendorId = SelectedVendor.Id,
            IsActive = false
        };

        SetBusy("Deactivating vendor...");
        var result = await _vendorCommandService.UpdateVendorStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleVendorStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate vendor.";
        }
    }

    private async Task HandleVendorStatusUpdateSuccess()
    {
        await LoadVendorsAsync();
        SelectedVendor = null;
        ActivateVendorCommand.NotifyCanExecuteChanged();
        DeactivateVendorCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected vendor.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteVendorCommand))]
    private async Task DeleteVendorAsync()
    {
        if (SelectedVendor is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this vendor?\n\nThis action hides the vendor from the application.",
            "Delete Vendor",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteVendorRequest
        {
            VendorId = SelectedVendor.Id
        };

        SetBusy("Deleting vendor...");
        var result = await _vendorCommandService.DeleteVendorAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadVendorsAsync();
            SelectedVendor = null;
            DeleteVendorCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete vendor.";
        }
    }

    private bool CanExecuteVendorCommand()
    {
        return SelectedVendor is not null;
    }

    /// <summary>
    /// Loads all vendors.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadVendorsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading vendors...");
        var vendors = await _vendorQueryService.GetAllVendorsAsync(cancellationToken);
        UpdateVendors(vendors);
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
            await SearchVendorsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches vendors based on the current search text.
    /// </summary>
    private async Task SearchVendorsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching vendors...");
        var vendors = await _vendorQueryService.SearchVendorsAsync(SearchText, cancellationToken);
        UpdateVendors(vendors);
        ClearBusy();
    }

    /// <summary>
    /// Updates the vendors collection on the UI thread.
    /// </summary>
    /// <param name="vendors">The vendors to update.</param>
    private void UpdateVendors(IEnumerable<VendorListItem> vendors)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateVendorsInternal(vendors);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateVendorsInternal(vendors));
        }
    }

    /// <summary>
    /// Updates the vendors collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="vendors">The vendors to update.</param>
    private void UpdateVendorsInternal(IEnumerable<VendorListItem> vendors)
    {
        Vendors.Clear();
        foreach (var vendor in vendors)
        {
            Vendors.Add(vendor);
        }
    }
}
