using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Reports.TdsReport.Contracts;
using veteran_logistic.Reports.TdsReport.DTOs;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Reports.TdsReport.ViewModels;

/// <summary>
/// ViewModel for the TDS Report screen.
/// </summary>
public sealed partial class TdsReportViewModel : ViewModelBase
{
    private readonly ITdsReportQueryService _tdsReportQueryService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private readonly ITdsReportPdfExporter _pdfExporter;
    private readonly ITdsReportExcelExporter _excelExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private string _sortBy = "paymentdate";
    private bool _sortAscending = true;
    private TdsReportFilter _filter = new();
    private CancellationTokenSource? _searchCancellationTokenSource;
    private string _groupBy = "customer";

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="TdsReportViewModel"/> class.
    /// </summary>
    /// <param name="tdsReportQueryService">The TDS report query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    /// <param name="pdfExporter">The PDF exporter.</param>
    /// <param name="excelExporter">The Excel exporter.</param>
    /// <param name="notificationService">The notification service.</param>
    public TdsReportViewModel(
        ITdsReportQueryService tdsReportQueryService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        IVehicleQueryService vehicleQueryService,
        IPaymentLocationQueryService paymentLocationQueryService,
        ITdsReportPdfExporter pdfExporter,
        ITdsReportExcelExporter excelExporter,
        INotificationService notificationService)
    {
        _tdsReportQueryService = tdsReportQueryService ?? throw new ArgumentNullException(nameof(tdsReportQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "TDS Report";
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

        await LoadDropdownDataAsync(cancellationToken);
        await LoadReportAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadReportAsync(cancellationToken);
        
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
    /// Gets the collection of TDS report items to display.
    /// </summary>
    public ObservableCollection<TdsReportItem> ReportItems { get; } = new();

    /// <summary>
    /// Gets the calculated totals for the report.
    /// </summary>
    public TdsReportTotals Totals { get; } = new();

    /// <summary>
    /// Gets the collection of grouped summary items.
    /// </summary>
    public ObservableCollection<TdsReportGroupSummary> GroupedSummaries { get; } = new();

    /// <summary>
    /// Gets the collection of customers for dropdown filters.
    /// </summary>
    public ObservableCollection<CustomerListItem> Customers { get; } = new();

    /// <summary>
    /// Gets the collection of vehicles for dropdown filters.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets the collection of payment locations for dropdown filters.
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
    /// Gets or sets the sort field.
    /// </summary>
    public string SortBy
    {
        get => _sortBy;
        set
        {
            if (SetProperty(ref _sortBy, value))
            {
                _ = LoadReportAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets whether sorting is ascending.
    /// </summary>
    public bool SortAscending
    {
        get => _sortAscending;
        set
        {
            if (SetProperty(ref _sortAscending, value))
            {
                _ = LoadReportAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets the filter criteria.
    /// </summary>
    public TdsReportFilter Filter
    {
        get => _filter;
        set => SetProperty(ref _filter, value);
    }

    /// <summary>
    /// Gets or sets the group by field.
    /// </summary>
    public string GroupBy
    {
        get => _groupBy;
        set
        {
            if (_groupBy != value)
            {
                _groupBy = value;
                OnPropertyChanged(nameof(GroupBy));
            }
        }
    }

    /// <summary>
    /// Command to refresh the report.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadReportAsync();
    }

    /// <summary>
    /// Command to apply filters.
    /// </summary>
    [RelayCommand]
    private async Task ApplyFiltersAsync()
    {
        await LoadReportAsync();
    }

    /// <summary>
    /// Command to clear all filters.
    /// </summary>
    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        Filter.Clear();
        OnPropertyChanged(nameof(Filter));
        await LoadReportAsync();
    }

    /// <summary>
    /// Command to sort by payment date.
    /// </summary>
    [RelayCommand]
    private async Task SortByPaymentDateAsync()
    {
        if (SortBy == "paymentdate")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "paymentdate";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by customer.
    /// </summary>
    [RelayCommand]
    private async Task SortByCustomerAsync()
    {
        if (SortBy == "customer")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "customer";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by beneficiary.
    /// </summary>
    [RelayCommand]
    private async Task SortByBeneficiaryAsync()
    {
        if (SortBy == "beneficiary")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "beneficiary";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by PAN.
    /// </summary>
    [RelayCommand]
    private async Task SortByPANAsync()
    {
        if (SortBy == "pan")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "pan";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by bank name.
    /// </summary>
    [RelayCommand]
    private async Task SortByBankNameAsync()
    {
        if (SortBy == "bankname")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "bankname";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by TDS amount.
    /// </summary>
    [RelayCommand]
    private async Task SortByTDSAmountAsync()
    {
        if (SortBy == "tdsamount")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "tdsamount";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by net payment.
    /// </summary>
    [RelayCommand]
    private async Task SortByNetPaymentAsync()
    {
        if (SortBy == "netpayment")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "netpayment";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to export to PDF.
    /// </summary>
    [RelayCommand]
    private async Task ExportToPdfAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Save TDS Report as PDF",
            FileName = $"TDSReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to PDF...");
            try
            {
                await _pdfExporter.ExportToPdfAsync(
                    ReportItems.ToList(),
                    Totals,
                    Filter,
                    saveFileDialog.FileName);
                
                await _notificationService.ShowSuccessAsync("Export Successful", "PDF exported successfully");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Export Failed", $"Failed to export PDF: {ex.Message}");
            }
            finally
            {
                ClearBusy();
            }
        }
    }

    /// <summary>
    /// Command to export to Excel.
    /// </summary>
    [RelayCommand]
    private async Task ExportToExcelAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx",
            Title = "Save TDS Report as Excel",
            FileName = $"TDSReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to Excel...");
            try
            {
                await _excelExporter.ExportToExcelAsync(
                    ReportItems.ToList(),
                    Totals,
                    Filter,
                    saveFileDialog.FileName);
                
                await _notificationService.ShowSuccessAsync("Export Successful", "Excel exported successfully");
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Export Failed", $"Failed to export Excel: {ex.Message}");
            }
            finally
            {
                ClearBusy();
            }
        }
    }

    /// <summary>
    /// Loads the TDS report data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadReportAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading report...");
        var (items, totals) = await _tdsReportQueryService.GetTdsReportAsync(
            Filter,
            SearchText,
            SortBy,
            SortAscending,
            cancellationToken);
        UpdateReportItems(items);
        UpdateTotals(totals);
        await LoadGroupedSummaryAsync(cancellationToken);
        ClearBusy();
    }

    /// <summary>
    /// Loads the grouped summary.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadGroupedSummaryAsync(CancellationToken cancellationToken = default)
    {
        var summaries = await _tdsReportQueryService.GetGroupedSummaryAsync(
            GroupBy,
            Filter,
            cancellationToken);
        UpdateGroupedSummaries(summaries);
    }

    /// <summary>
    /// Loads dropdown data for filters.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadDropdownDataAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading filter data...");
        
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
        var vehicles = await _vehicleQueryService.GetAllVehiclesAsync(cancellationToken);
        var paymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync(cancellationToken);

        UpdateCustomers(customers);
        UpdateVehicles(vehicles);
        UpdatePaymentLocations(paymentLocations);
        
        ClearBusy();
    }

    /// <summary>
    /// Updates the customers collection on the UI thread.
    /// </summary>
    private void UpdateCustomers(IEnumerable<CustomerListItem> customers)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            });
        }
    }

    /// <summary>
    /// Updates the vehicles collection on the UI thread.
    /// </summary>
    private void UpdateVehicles(IEnumerable<VehicleListItem> vehicles)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            Vehicles.Clear();
            foreach (var vehicle in vehicles)
            {
                Vehicles.Add(vehicle);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                Vehicles.Clear();
                foreach (var vehicle in vehicles)
                {
                    Vehicles.Add(vehicle);
                }
            });
        }
    }

    /// <summary>
    /// Updates the payment locations collection on the UI thread.
    /// </summary>
    private void UpdatePaymentLocations(IEnumerable<PaymentLocationListItem> paymentLocations)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            PaymentLocations.Clear();
            foreach (var pl in paymentLocations)
            {
                PaymentLocations.Add(pl);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                PaymentLocations.Clear();
                foreach (var pl in paymentLocations)
                {
                    PaymentLocations.Add(pl);
                }
            });
        }
    }

    /// <summary>
    /// Updates the grouped summaries collection on the UI thread.
    /// </summary>
    private void UpdateGroupedSummaries(IEnumerable<TdsReportGroupSummary> summaries)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            GroupedSummaries.Clear();
            foreach (var summary in summaries)
            {
                GroupedSummaries.Add(summary);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                GroupedSummaries.Clear();
                foreach (var summary in summaries)
                {
                    GroupedSummaries.Add(summary);
                }
            });
        }
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
            await LoadReportAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Updates the report items collection on the UI thread.
    /// </summary>
    /// <param name="items">The report items to update.</param>
    private void UpdateReportItems(IEnumerable<TdsReportItem> items)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateReportItemsInternal(items);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateReportItemsInternal(items));
        }
    }

    /// <summary>
    /// Updates the report items collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="items">The report items to update.</param>
    private void UpdateReportItemsInternal(IEnumerable<TdsReportItem> items)
    {
        ReportItems.Clear();
        foreach (var item in items)
        {
            ReportItems.Add(item);
        }
    }

    /// <summary>
    /// Updates the totals.
    /// </summary>
    /// <param name="totals">The totals to update.</param>
    private void UpdateTotals(TdsReportTotals totals)
    {
        Totals.RecordCount = totals.RecordCount;
        Totals.TotalChallanAmount = totals.TotalChallanAmount;
        Totals.TotalTDSAmount = totals.TotalTDSAmount;
        Totals.TotalSurcharge = totals.TotalSurcharge;
        Totals.TotalAdminCharge = totals.TotalAdminCharge;
        Totals.TotalNetPayment = totals.TotalNetPayment;
        Totals.AverageTDSAmount = totals.AverageTDSAmount;
        Totals.HighestTDSAmount = totals.HighestTDSAmount;
        Totals.LowestTDSAmount = totals.LowestTDSAmount;

        OnPropertyChanged(nameof(Totals));
    }
}
