using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Reports.ConsolidatedReport.Contracts;
using veteran_logistic.Reports.ConsolidatedReport.DTOs;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Reports.ConsolidatedReport.ViewModels;

/// <summary>
/// ViewModel for the Consolidated Report screen.
/// </summary>
public sealed partial class ConsolidatedReportViewModel : ViewModelBase
{
    private readonly IConsolidatedReportQueryService _consolidatedReportQueryService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly IConsolidatedReportPdfExporter _pdfExporter;
    private readonly IConsolidatedReportExcelExporter _excelExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private string _sortBy = "loadingdate";
    private bool _sortAscending = true;
    private ConsolidatedReportFilter _filter = new();
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
    /// Initializes a new instance of the <see cref="ConsolidatedReportViewModel"/> class.
    /// </summary>
    /// <param name="consolidatedReportQueryService">The consolidated report query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="sourceDestinationQueryService">The source destination query service.</param>
    /// <param name="pdfExporter">The PDF exporter.</param>
    /// <param name="excelExporter">The Excel exporter.</param>
    /// <param name="notificationService">The notification service.</param>
    public ConsolidatedReportViewModel(
        IConsolidatedReportQueryService consolidatedReportQueryService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        ISourceDestinationQueryService sourceDestinationQueryService,
        IConsolidatedReportPdfExporter pdfExporter,
        IConsolidatedReportExcelExporter excelExporter,
        INotificationService notificationService)
    {
        _consolidatedReportQueryService = consolidatedReportQueryService ?? throw new ArgumentNullException(nameof(consolidatedReportQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "Consolidated Report";
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
    /// Gets the collection of consolidated report items to display.
    /// </summary>
    public ObservableCollection<ConsolidatedReportItem> ReportItems { get; } = new();

    /// <summary>
    /// Gets the calculated totals for the report.
    /// </summary>
    public ConsolidatedReportTotals Totals { get; } = new();

    /// <summary>
    /// Gets the summary cards (KPIs) for the report.
    /// </summary>
    public ConsolidatedReportSummaryCards SummaryCards { get; } = new();

    /// <summary>
    /// Gets the collection of consignors for dropdown filters.
    /// </summary>
    public ObservableCollection<CustomerListItem> Consignors { get; } = new();

    /// <summary>
    /// Gets the collection of consignees for dropdown filters.
    /// </summary>
    public ObservableCollection<CustomerListItem> Consignees { get; } = new();

    /// <summary>
    /// Gets the collection of sources for dropdown filters.
    /// </summary>
    public ObservableCollection<SourceDestinationListItem> Sources { get; } = new();

    /// <summary>
    /// Gets the collection of destinations for dropdown filters.
    /// </summary>
    public ObservableCollection<SourceDestinationListItem> Destinations { get; } = new();

    /// <summary>
    /// Gets the SAR filter options.
    /// </summary>
    public ObservableCollection<string> SARFilterOptions { get; } = new()
    {
        "Show All Records",
        "SAR- unloaded trips",
        "SAR-not unloaded trips",
        "SAR-paid",
        "SAR-unpaid",
        "SAR-billed",
        "SAR-not billed"
    };

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
    public ConsolidatedReportFilter Filter
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
        await LoadDropdownDataAsync(CancellationToken.None);
        await LoadReportAsync(CancellationToken.None);
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
    /// Command to clear filters.
    /// </summary>
    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        Filter.Clear();
        OnPropertyChanged(nameof(Filter));
        await LoadReportAsync();
    }

    /// <summary>
    /// Command to export to PDF.
    /// </summary>
    [RelayCommand]
    private async Task ExportToPdfAsync()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf",
            Title = "Export Consolidated Report to PDF",
            FileName = $"ConsolidatedReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to PDF...");
            try
            {
                await _pdfExporter.ExportToPdfAsync(
                    ReportItems.ToList(),
                    Totals,
                    SummaryCards,
                    Filter,
                    saveFileDialog.FileName);
                
                _ = _notificationService.ShowSuccessAsync("Export Complete", "PDF exported successfully");
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
            Filter = "Excel files (*.xlsx)|*.xlsx",
            Title = "Export Consolidated Report to Excel",
            FileName = $"ConsolidatedReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            SetBusy("Exporting to Excel...");
            try
            {
                await _excelExporter.ExportToExcelAsync(
                    ReportItems.ToList(),
                    Totals,
                    SummaryCards,
                    Filter,
                    saveFileDialog.FileName);
                
                _ = _notificationService.ShowSuccessAsync("Export Complete", "Excel exported successfully");
            }
            finally
            {
                ClearBusy();
            }
        }
    }

    private async Task LoadDropdownDataAsync(CancellationToken cancellationToken)
    {
        await LoadConsignorsAsync(cancellationToken);
        await LoadConsigneesAsync(cancellationToken);
        await LoadSourcesAsync(cancellationToken);
        await LoadDestinationsAsync(cancellationToken);
    }

    private async Task LoadConsignorsAsync(CancellationToken cancellationToken)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
        Consignors.Clear();
        foreach (var customer in customers)
        {
            Consignors.Add(customer);
        }
    }

    private async Task LoadConsigneesAsync(CancellationToken cancellationToken)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
        Consignees.Clear();
        foreach (var customer in customers)
        {
            Consignees.Add(customer);
        }
    }

    private async Task LoadSourcesAsync(CancellationToken cancellationToken)
    {
        try
        {
            var sourceDestinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken);
            Sources.Clear();
            foreach (var sourceDestination in sourceDestinations)
            {
                Sources.Add(sourceDestination);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading sources: {ex.Message}");
        }
    }

    private async Task LoadDestinationsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var sourceDestinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken);
            Destinations.Clear();
            foreach (var sourceDestination in sourceDestinations)
            {
                Destinations.Add(sourceDestination);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading destinations: {ex.Message}");
        }
    }

    private async Task LoadReportAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading report...");
        try
        {
            var (items, totals, summaryCards) = await _consolidatedReportQueryService.GetConsolidatedReportAsync(
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
            Totals.TotalLoadingWeight = totals.TotalLoadingWeight;
            Totals.TotalUnloadingWeight = totals.TotalUnloadingWeight;
            Totals.TotalShortageWeight = totals.TotalShortageWeight;
            Totals.TotalLoadingAmount = totals.TotalLoadingAmount;
            Totals.TotalChallanAmount = totals.TotalChallanAmount;
            Totals.TotalNetPayment = totals.TotalNetPayment;
            Totals.TotalTDSAmount = totals.TotalTDSAmount;
            Totals.TotalBills = totals.TotalBills;
            Totals.AverageNetPayment = totals.AverageNetPayment;

            SummaryCards.TotalTransactions = summaryCards.TotalTransactions;
            SummaryCards.LoadingOnly = summaryCards.LoadingOnly;
            SummaryCards.PendingUnloading = summaryCards.PendingUnloading;
            SummaryCards.PendingPayment = summaryCards.PendingPayment;
            SummaryCards.PendingBilling = summaryCards.PendingBilling;
            SummaryCards.Completed = summaryCards.Completed;
            SummaryCards.TotalRevenue = summaryCards.TotalRevenue;
            SummaryCards.TotalNetPayment = summaryCards.TotalNetPayment;
            SummaryCards.TotalTDS = summaryCards.TotalTDS;
        }
        finally
        {
            ClearBusy();
        }
    }

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
            // Search was debounced, ignore
        }
    }
}
