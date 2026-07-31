using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Reports.PaymentReport.Contracts;
using veteran_logistic.Reports.PaymentReport.DTOs;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Reports.PaymentReport.ViewModels;

/// <summary>
/// ViewModel for the Payment Report screen.
/// </summary>
public sealed partial class PaymentReportViewModel : ViewModelBase
{
    private readonly IPaymentReportQueryService _paymentReportQueryService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IMaterialQueryService _materialQueryService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private readonly IVehicleOwnerQueryService _vehicleOwnerQueryService;
    private readonly IPaymentReportPdfExporter _pdfExporter;
    private readonly IPaymentReportExcelExporter _excelExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private string _sortBy = "paymentdate";
    private bool _sortAscending = true;
    private PaymentReportFilter _filter = new();
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
    /// Initializes a new instance of the <see cref="PaymentReportViewModel"/> class.
    /// </summary>
    /// <param name="paymentReportQueryService">The payment report query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="materialQueryService">The material query service.</param>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    /// <param name="vehicleOwnerQueryService">The vehicle owner query service.</param>
    /// <param name="pdfExporter">The PDF exporter.</param>
    /// <param name="excelExporter">The Excel exporter.</param>
    /// <param name="notificationService">The notification service.</param>
    public PaymentReportViewModel(
        IPaymentReportQueryService paymentReportQueryService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        IVehicleQueryService vehicleQueryService,
        IMaterialQueryService materialQueryService,
        IPaymentLocationQueryService paymentLocationQueryService,
        IVehicleOwnerQueryService vehicleOwnerQueryService,
        IPaymentReportPdfExporter pdfExporter,
        IPaymentReportExcelExporter excelExporter,
        INotificationService notificationService)
    {
        _paymentReportQueryService = paymentReportQueryService ?? throw new ArgumentNullException(nameof(paymentReportQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));
        _vehicleOwnerQueryService = vehicleOwnerQueryService ?? throw new ArgumentNullException(nameof(vehicleOwnerQueryService));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "Payment Report";
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
    /// Gets the collection of payment report items to display.
    /// </summary>
    public ObservableCollection<PaymentReportItem> ReportItems { get; } = new();

    /// <summary>
    /// Gets the calculated totals for the report.
    /// </summary>
    public PaymentReportTotals Totals { get; } = new();

    /// <summary>
    /// Gets the collection of customers for dropdown filters.
    /// </summary>
    public ObservableCollection<CustomerListItem> Customers { get; } = new();

    /// <summary>
    /// Gets the collection of vehicles for dropdown filters.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets the collection of materials for dropdown filters.
    /// </summary>
    public ObservableCollection<MaterialListItem> Materials { get; } = new();

    /// <summary>
    /// Gets the collection of payment locations for dropdown filters.
    /// </summary>
    public ObservableCollection<PaymentLocationListItem> PaymentLocations { get; } = new();

    /// <summary>
    /// Gets the collection of vehicle owners for dropdown filters.
    /// </summary>
    public ObservableCollection<VehicleOwnerListItem> VehicleOwners { get; } = new();

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
    public PaymentReportFilter Filter
    {
        get => _filter;
        set => SetProperty(ref _filter, value);
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
    /// Command to sort by challan number.
    /// </summary>
    [RelayCommand]
    private async Task SortByChallanNumberAsync()
    {
        if (SortBy == "challannumber")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "challannumber";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by vehicle.
    /// </summary>
    [RelayCommand]
    private async Task SortByVehicleAsync()
    {
        if (SortBy == "vehicle")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "vehicle";
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
    /// Command to sort by payment type.
    /// </summary>
    [RelayCommand]
    private async Task SortByPaymentTypeAsync()
    {
        if (SortBy == "paymenttype")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "paymenttype";
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
    /// Command to sort by driver commission.
    /// </summary>
    [RelayCommand]
    private async Task SortByDriverCommissionAsync()
    {
        if (SortBy == "drivercommission")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "drivercommission";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by challan amount.
    /// </summary>
    [RelayCommand]
    private async Task SortByChallanAmountAsync()
    {
        if (SortBy == "challanamount")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "challanamount";
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
            Title = "Save Payment Report as PDF",
            FileName = $"PaymentReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
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
            Title = "Save Payment Report as Excel",
            FileName = $"PaymentReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
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
    /// Loads the payment report data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadReportAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading report...");
        var (items, totals) = await _paymentReportQueryService.GetPaymentReportAsync(
            Filter,
            SearchText,
            SortBy,
            SortAscending,
            cancellationToken);
        UpdateReportItems(items);
        UpdateTotals(totals);
        ClearBusy();
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
        var materials = await _materialQueryService.GetAllMaterialsAsync(cancellationToken);
        var paymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync(cancellationToken);
        var vehicleOwners = await _vehicleOwnerQueryService.GetAllVehicleOwnersAsync(cancellationToken);

        UpdateCustomers(customers);
        UpdateVehicles(vehicles);
        UpdateMaterials(materials);
        UpdatePaymentLocations(paymentLocations);
        UpdateVehicleOwners(vehicleOwners);
        
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
    /// Updates the materials collection on the UI thread.
    /// </summary>
    private void UpdateMaterials(IEnumerable<MaterialListItem> materials)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            Materials.Clear();
            foreach (var material in materials)
            {
                Materials.Add(material);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                Materials.Clear();
                foreach (var material in materials)
                {
                    Materials.Add(material);
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
    /// Updates the vehicle owners collection on the UI thread.
    /// </summary>
    private void UpdateVehicleOwners(IEnumerable<VehicleOwnerListItem> vehicleOwners)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            VehicleOwners.Clear();
            foreach (var vo in vehicleOwners)
            {
                VehicleOwners.Add(vo);
            }
        }
        else
        {
            dispatcher.Invoke(() => {
                VehicleOwners.Clear();
                foreach (var vo in vehicleOwners)
                {
                    VehicleOwners.Add(vo);
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
    private void UpdateReportItems(IEnumerable<PaymentReportItem> items)
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
    private void UpdateReportItemsInternal(IEnumerable<PaymentReportItem> items)
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
    private void UpdateTotals(PaymentReportTotals totals)
    {
        Totals.RecordCount = totals.RecordCount;
        Totals.TotalLoadingWeight = totals.TotalLoadingWeight;
        Totals.TotalUnloadingWeight = totals.TotalUnloadingWeight;
        Totals.TotalDriverCommission = totals.TotalDriverCommission;
        Totals.TotalChallanAmount = totals.TotalChallanAmount;
        Totals.TotalTDSAmount = totals.TotalTDSAmount;
        Totals.TotalSurchargeAmount = totals.TotalSurchargeAmount;
        Totals.TotalAdminCharge = totals.TotalAdminCharge;
        Totals.TotalNetPayment = totals.TotalNetPayment;

        OnPropertyChanged(nameof(Totals));
    }
}
