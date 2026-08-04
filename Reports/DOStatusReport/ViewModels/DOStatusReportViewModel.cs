using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Reports.DOStatusReport.Contracts;
using veteran_logistic.Reports.DOStatusReport.DTOs;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Reports.DOStatusReport.ViewModels;

/// <summary>
/// ViewModel for the DO Status Report screen.
/// </summary>
public sealed partial class DOStatusReportViewModel : ViewModelBase
{
    private readonly IDOStatusReportQueryService _doStatusReportQueryService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IMaterialQueryService _materialQueryService;
    private readonly IDOStatusReportPdfExporter _pdfExporter;
    private readonly IDOStatusReportExcelExporter _excelExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private string _sortBy = "loadingdate";
    private bool _sortAscending = true;
    private DOStatusReportFilter _filter = new();
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
    /// Initializes a new instance of the <see cref="DOStatusReportViewModel"/> class.
    /// </summary>
    /// <param name="doStatusReportQueryService">The DO status report query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="sourceDestinationQueryService">The source/destination query service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="materialQueryService">The material query service.</param>
    /// <param name="pdfExporter">The PDF exporter.</param>
    /// <param name="excelExporter">The Excel exporter.</param>
    /// <param name="notificationService">The notification service.</param>
    public DOStatusReportViewModel(
        IDOStatusReportQueryService doStatusReportQueryService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        ISourceDestinationQueryService sourceDestinationQueryService,
        IVehicleQueryService vehicleQueryService,
        IMaterialQueryService materialQueryService,
        IDOStatusReportPdfExporter pdfExporter,
        IDOStatusReportExcelExporter excelExporter,
        INotificationService notificationService)
    {
        _doStatusReportQueryService = doStatusReportQueryService ?? throw new ArgumentNullException(nameof(doStatusReportQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "DO Status Report";
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
    /// Gets the collection of DO status report items to display.
    /// </summary>
    public ObservableCollection<DOStatusReportItem> ReportItems { get; } = new();

    /// <summary>
    /// Gets the calculated summary cards for the report.
    /// </summary>
    public DOStatusReportSummaryCards SummaryCards { get; } = new();

    /// <summary>
    /// Gets the calculated totals for the report.
    /// </summary>
    public DOStatusReportTotals Totals { get; } = new();

    /// <summary>
    /// Gets the collection of customers for dropdown filters.
    /// </summary>
    public ObservableCollection<CustomerListItem> Customers { get; } = new();

    /// <summary>
    /// Gets the collection of source/destinations for dropdown filters.
    /// </summary>
    public ObservableCollection<SourceDestinationListItem> SourceDestinations { get; } = new();

    /// <summary>
    /// Gets the collection of vehicles for dropdown filters.
    /// </summary>
    public ObservableCollection<VehicleListItem> Vehicles { get; } = new();

    /// <summary>
    /// Gets the collection of materials for dropdown filters.
    /// </summary>
    public ObservableCollection<MaterialListItem> Materials { get; } = new();

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
    public DOStatusReportFilter Filter
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
    /// Command to sort by loading date.
    /// </summary>
    [RelayCommand]
    private async Task SortByLoadingDateAsync()
    {
        if (SortBy == "loadingdate")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "loadingdate";
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
    /// Command to sort by status.
    /// </summary>
    [RelayCommand]
    private async Task SortByStatusAsync()
    {
        if (SortBy == "status")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "status";
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
            Title = "Save DO Status Report as PDF",
            FileName = $"DOStatusReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to PDF...");
            try
            {
                await _pdfExporter.ExportToPdfAsync(
                    ReportItems.ToList(),
                    SummaryCards,
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
            Title = "Save DO Status Report as Excel",
            FileName = $"DOStatusReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to Excel...");
            try
            {
                await _excelExporter.ExportToExcelAsync(
                    ReportItems.ToList(),
                    SummaryCards,
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
    /// Command to print the report.
    /// </summary>
    [RelayCommand]
    private async Task PrintAsync()
    {
        await ExportToPdfAsync();
    }

    /// <summary>
    /// Loads the DO status report data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadReportAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading report...");
        var (items, summaryCards, totals) = await _doStatusReportQueryService.GetDOStatusReportAsync(
            Filter,
            SearchText,
            SortBy,
            SortAscending,
            cancellationToken);
        UpdateReportItems(items);
        UpdateSummaryCards(summaryCards);
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
        var sourceDestinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken);
        var vehicles = await _vehicleQueryService.GetAllVehiclesAsync(cancellationToken);
        var materials = await _materialQueryService.GetAllMaterialsAsync(cancellationToken);

        UpdateCustomers(customers);
        UpdateSourceDestinations(sourceDestinations);
        UpdateVehicles(vehicles);
        UpdateMaterials(materials);
        
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
            dispatcher.Invoke(() =>
            {
                Customers.Clear();
                foreach (var customer in customers)
                {
                    Customers.Add(customer);
                }
            });
        }
    }

    /// <summary>
    /// Updates the source/destinations collection on the UI thread.
    /// </summary>
    private void UpdateSourceDestinations(IEnumerable<SourceDestinationListItem> sourceDestinations)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            SourceDestinations.Clear();
            foreach (var sourceDestination in sourceDestinations)
            {
                SourceDestinations.Add(sourceDestination);
            }
        }
        else
        {
            dispatcher.Invoke(() =>
            {
                SourceDestinations.Clear();
                foreach (var sourceDestination in sourceDestinations)
                {
                    SourceDestinations.Add(sourceDestination);
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
            dispatcher.Invoke(() =>
            {
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
            dispatcher.Invoke(() =>
            {
                Materials.Clear();
                foreach (var material in materials)
                {
                    Materials.Add(material);
                }
            });
        }
    }

    /// <summary>
    /// Updates the report items collection on the UI thread.
    /// </summary>
    private void UpdateReportItems(IEnumerable<DOStatusReportItem> items)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            ReportItems.Clear();
            foreach (var item in items)
            {
                ReportItems.Add(item);
            }
        }
        else
        {
            dispatcher.Invoke(() =>
            {
                ReportItems.Clear();
                foreach (var item in items)
                {
                    ReportItems.Add(item);
                }
            });
        }
    }

    /// <summary>
    /// Updates the summary cards on the UI thread.
    /// </summary>
    private void UpdateSummaryCards(DOStatusReportSummaryCards summaryCards)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            SummaryCards.TotalDO = summaryCards.TotalDO;
            SummaryCards.TodayLoading = summaryCards.TodayLoading;
            SummaryCards.RunningDO = summaryCards.RunningDO;
            SummaryCards.CompletedDO = summaryCards.CompletedDO;
            SummaryCards.PaymentPending = summaryCards.PaymentPending;
            SummaryCards.BillPending = summaryCards.BillPending;
        }
        else
        {
            dispatcher.Invoke(() =>
            {
                SummaryCards.TotalDO = summaryCards.TotalDO;
                SummaryCards.TodayLoading = summaryCards.TodayLoading;
                SummaryCards.RunningDO = summaryCards.RunningDO;
                SummaryCards.CompletedDO = summaryCards.CompletedDO;
                SummaryCards.PaymentPending = summaryCards.PaymentPending;
                SummaryCards.BillPending = summaryCards.BillPending;
            });
        }
    }

    /// <summary>
    /// Updates the totals on the UI thread.
    /// </summary>
    private void UpdateTotals(DOStatusReportTotals totals)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            Totals.TotalRecords = totals.TotalRecords;
            Totals.TotalLoadingWeight = totals.TotalLoadingWeight;
            Totals.TotalUnloadingWeight = totals.TotalUnloadingWeight;
            Totals.TotalShortageWeight = totals.TotalShortageWeight;
            Totals.TotalGrossAmount = totals.TotalGrossAmount;
            Totals.TotalChallanMoney = totals.TotalChallanMoney;
            Totals.TotalPendingAmount = totals.TotalPendingAmount;
        }
        else
        {
            dispatcher.Invoke(() =>
            {
                Totals.TotalRecords = totals.TotalRecords;
                Totals.TotalLoadingWeight = totals.TotalLoadingWeight;
                Totals.TotalUnloadingWeight = totals.TotalUnloadingWeight;
                Totals.TotalShortageWeight = totals.TotalShortageWeight;
                Totals.TotalGrossAmount = totals.TotalGrossAmount;
                Totals.TotalChallanMoney = totals.TotalChallanMoney;
                Totals.TotalPendingAmount = totals.TotalPendingAmount;
            });
        }
    }

    /// <summary>
    /// Performs debounced search with 300ms delay.
    /// </summary>
    private async Task DebouncedSearchAsync()
    {
        _searchCancellationTokenSource?.Cancel();
        _searchCancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(300, _searchCancellationTokenSource.Token);
            await LoadReportAsync(_searchCancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            // Search was cancelled by new input
        }
    }
}
