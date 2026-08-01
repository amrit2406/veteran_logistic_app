using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Reports.PartyBillingReport.Contracts;
using veteran_logistic.Reports.PartyBillingReport.DTOs;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Reports.PartyBillingReport.ViewModels;

/// <summary>
/// ViewModel for the Party Billing Report screen.
/// </summary>
public sealed partial class PartyBillingReportViewModel : ViewModelBase
{
    private readonly IPartyBillingReportQueryService _partyBillingReportQueryService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly IMaterialQueryService _materialQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly IPartyBillingReportPdfExporter _pdfExporter;
    private readonly IPartyBillingReportExcelExporter _excelExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private string _sortBy = "billdate";
    private bool _sortAscending = true;
    private PartyBillingReportFilter _filter = new();
    private CancellationTokenSource? _searchCancellationTokenSource;
    private PartyBillingReportItem? _selectedBill;

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillingReportViewModel"/> class.
    /// </summary>
    /// <param name="partyBillingReportQueryService">The party billing report query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="vehicleQueryService">The vehicle query service.</param>
    /// <param name="materialQueryService">The material query service.</param>
    /// <param name="sourceDestinationQueryService">The source destination query service.</param>
    /// <param name="pdfExporter">The PDF exporter.</param>
    /// <param name="excelExporter">The Excel exporter.</param>
    /// <param name="notificationService">The notification service.</param>
    public PartyBillingReportViewModel(
        IPartyBillingReportQueryService partyBillingReportQueryService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        IVehicleQueryService vehicleQueryService,
        IMaterialQueryService materialQueryService,
        ISourceDestinationQueryService sourceDestinationQueryService,
        IPartyBillingReportPdfExporter pdfExporter,
        IPartyBillingReportExcelExporter excelExporter,
        INotificationService notificationService)
    {
        _partyBillingReportQueryService = partyBillingReportQueryService ?? throw new ArgumentNullException(nameof(partyBillingReportQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "Party Wise Billing Report";
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
    /// Gets the collection of party billing report summary items to display.
    /// </summary>
    public ObservableCollection<PartyBillingReportItem> ReportItems { get; } = new();

    /// <summary>
    /// Gets the collection of party billing report detail items to display.
    /// </summary>
    public ObservableCollection<PartyBillingReportDetailItem> DetailItems { get; } = new();

    /// <summary>
    /// Gets the calculated totals for the report.
    /// </summary>
    public PartyBillingReportTotals Totals { get; } = new();

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
    /// Gets the collection of destinations for dropdown filters.
    /// </summary>
    public ObservableCollection<SourceDestinationListItem> Destinations { get; } = new();

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
    public PartyBillingReportFilter Filter
    {
        get => _filter;
        set => SetProperty(ref _filter, value);
    }

    /// <summary>
    /// Gets or sets the selected bill for detail view.
    /// </summary>
    public PartyBillingReportItem? SelectedBill
    {
        get => _selectedBill;
        set
        {
            if (SetProperty(ref _selectedBill, value))
            {
                _ = LoadBillDetailsAsync();
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
    /// Command to sort by bill date.
    /// </summary>
    [RelayCommand]
    private async Task SortByBillDateAsync()
    {
        if (SortBy == "billdate")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "billdate";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by bill number.
    /// </summary>
    [RelayCommand]
    private async Task SortByBillNumberAsync()
    {
        if (SortBy == "billnumber")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "billnumber";
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
    /// Command to sort by third party.
    /// </summary>
    [RelayCommand]
    private async Task SortByThirdPartyAsync()
    {
        if (SortBy == "thirdparty")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "thirdparty";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by total challans.
    /// </summary>
    [RelayCommand]
    private async Task SortByTotalChallansAsync()
    {
        if (SortBy == "totalchallans")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "totalchallans";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by total weight.
    /// </summary>
    [RelayCommand]
    private async Task SortByTotalWeightAsync()
    {
        if (SortBy == "totalweight")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "totalweight";
            SortAscending = true;
        }
    }

    /// <summary>
    /// Command to sort by total amount.
    /// </summary>
    [RelayCommand]
    private async Task SortByTotalAmountAsync()
    {
        if (SortBy == "totalamount")
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortBy = "totalamount";
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
            Title = "Save Party Billing Report as PDF",
            FileName = $"PartyBillingReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to PDF...");
            try
            {
                await _pdfExporter.ExportToPdfAsync(
                    ReportItems.ToList(),
                    DetailItems.ToList(),
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
            Title = "Save Party Billing Report as Excel",
            FileName = $"PartyBillingReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to Excel...");
            try
            {
                await _excelExporter.ExportToExcelAsync(
                    ReportItems.ToList(),
                    DetailItems.ToList(),
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
    /// Loads the party billing report summary data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadReportAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading report...");
        try
        {
            var (items, totals) = await _partyBillingReportQueryService.GetPartyBillingReportAsync(
                Filter,
                SearchText,
                SortBy,
                SortAscending,
                cancellationToken);

            ReportItems.Clear();
            foreach (var item in items)
            {
                ReportItems.Add(item);
            }

            Totals.RecordCount = totals.RecordCount;
            Totals.TotalBills = totals.TotalBills;
            Totals.TotalChallans = totals.TotalChallans;
            Totals.TotalLoadingWeight = totals.TotalLoadingWeight;
            Totals.TotalGrossAmount = totals.TotalGrossAmount;
            Totals.AverageBillAmount = totals.AverageBillAmount;

            // Clear details when report reloads
            DetailItems.Clear();
            SelectedBill = null;
        }
        finally
        {
            ClearBusy();
        }
    }

    /// <summary>
    /// Loads the bill details for the selected bill.
    /// </summary>
    private async Task LoadBillDetailsAsync()
    {
        if (SelectedBill == null)
        {
            DetailItems.Clear();
            return;
        }

        SetBusy("Loading bill details...");
        try
        {
            var details = await _partyBillingReportQueryService.GetPartyBillingReportDetailsAsync(
                SelectedBill.Id,
                default);

            DetailItems.Clear();
            foreach (var detail in details)
            {
                DetailItems.Add(detail);
            }
        }
        finally
        {
            ClearBusy();
        }
    }

    /// <summary>
    /// Loads dropdown data for filters.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadDropdownDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            var vehicles = await _vehicleQueryService.GetAllVehiclesAsync(cancellationToken);
            Vehicles.Clear();
            foreach (var vehicle in vehicles)
            {
                Vehicles.Add(vehicle);
            }

            var materials = await _materialQueryService.GetAllMaterialsAsync(cancellationToken);
            Materials.Clear();
            foreach (var material in materials)
            {
                Materials.Add(material);
            }

            var destinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken);
            Destinations.Clear();
            foreach (var destination in destinations)
            {
                Destinations.Add(destination);
            }
        }
        catch (Exception ex)
        {
            await _notificationService.ShowErrorAsync("Load Failed", $"Failed to load dropdown data: {ex.Message}");
        }
    }

    /// <summary>
    /// Performs debounced search.
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
            // Search was cancelled, ignore
        }
    }
}
