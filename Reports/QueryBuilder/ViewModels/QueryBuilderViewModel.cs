using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using veteran_logistic.Reports.QueryBuilder.Contracts;
using veteran_logistic.Reports.QueryBuilder.DTOs;
using veteran_logistic.Reports.QueryBuilder.Metadata;
using veteran_logistic.Reports.QueryBuilder.Models;
using Microsoft.Win32;
using System.Windows;
using veteran_logistic.Services.Notification;
using veteran_logistic.Reports.QueryBuilder.Export.Excel;
using veteran_logistic.Reports.QueryBuilder.Export.Pdf;
using veteran_logistic.Reports.QueryBuilder.Export.Csv;
using System.IO;

namespace veteran_logistic.Reports.QueryBuilder.ViewModels;

/// <summary>
/// ViewModel for the Query Builder screen.
/// </summary>
public sealed partial class QueryBuilderViewModel : ViewModelBase
{
    private readonly IQueryEngine _queryEngine;
    private readonly INavigationService _navigationService;
    private readonly IQueryBuilderExcelExporter _excelExporter;
    private readonly IQueryBuilderPdfExporter _pdfExporter;
    private readonly IQueryBuilderCsvExporter _csvExporter;
    private readonly INotificationService _notificationService;
    private string _searchText = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;
    private QueryDefinition _queryDefinition = new();
    private ModuleMetadata? _selectedModule;
    private FieldMetadata? _selectedAvailableColumn;
    private FieldMetadata? _selectedSelectedColumn;
    private QueryFilter? _selectedFilter;
    private QuerySort? _selectedSort;
    private FieldMetadata? _selectedGroupField;
    private QueryAggregate? _selectedAggregate;
    private FieldMetadata? _selectedAggregateField;
    private bool _isBusy;
    private string _validationMessage = string.Empty;
    private QueryResult? _queryResult;

    public IAsyncRelayCommand GoBackCommand { get; }
    public bool CanGoBack => _navigationService.CanGoBack;

    public QueryBuilderViewModel(
        IQueryEngine queryEngine,
        INavigationService navigationService,
        IQueryBuilderExcelExporter excelExporter,
        IQueryBuilderPdfExporter pdfExporter,
        IQueryBuilderCsvExporter csvExporter,
        INotificationService notificationService)
    {
        _queryEngine = queryEngine ?? throw new ArgumentNullException(nameof(queryEngine));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _excelExporter = excelExporter ?? throw new ArgumentNullException(nameof(excelExporter));
        _pdfExporter = pdfExporter ?? throw new ArgumentNullException(nameof(pdfExporter));
        _csvExporter = csvExporter ?? throw new ArgumentNullException(nameof(csvExporter));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "Query Builder";
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);

        AvailableModules = new ObservableCollection<ModuleMetadata>(QueryMetadataProvider.GetAllModules());
        AvailableColumns = new ObservableCollection<FieldMetadata>();
        SelectedColumns = new ObservableCollection<FieldMetadata>();
        Filters = new ObservableCollection<QueryFilter>();
        Sorts = new ObservableCollection<QuerySort>();
        Aggregates = new ObservableCollection<QueryAggregate>();
        ResultItems = new ObservableCollection<QueryResultItem>();
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

        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
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

    public ObservableCollection<ModuleMetadata> AvailableModules { get; }
    public ObservableCollection<FieldMetadata> AvailableColumns { get; }
    public ObservableCollection<FieldMetadata> SelectedColumns { get; }
    public ObservableCollection<QueryFilter> Filters { get; }
    public ObservableCollection<QuerySort> Sorts { get; }
    public ObservableCollection<QueryAggregate> Aggregates { get; }
    public ObservableCollection<QueryResultItem> ResultItems { get; }

    public ModuleMetadata? SelectedModule
    {
        get => _selectedModule;
        set
        {
            if (_selectedModule != value)
            {
                _selectedModule = value;
                OnPropertyChanged(nameof(SelectedModule));
                
                if (value != null)
                {
                    AvailableColumns.Clear();
                    foreach (var fieldItem in value.Fields)
                    {
                        AvailableColumns.Add(fieldItem);
                    }
                    SelectedColumns.Clear();
                    _queryDefinition.ModuleId = value.ModuleId;
                    _queryDefinition.SelectedColumns.Clear();
                }
            }
        }
    }

    public FieldMetadata? SelectedAvailableColumn
    {
        get => _selectedAvailableColumn;
        set => SetProperty(ref _selectedAvailableColumn, value);
    }

    public FieldMetadata? SelectedSelectedColumn
    {
        get => _selectedSelectedColumn;
        set => SetProperty(ref _selectedSelectedColumn, value);
    }

    public QueryFilter? SelectedFilter
    {
        get => _selectedFilter;
        set => SetProperty(ref _selectedFilter, value);
    }

    public QuerySort? SelectedSort
    {
        get => _selectedSort;
        set => SetProperty(ref _selectedSort, value);
    }

    public FieldMetadata? SelectedGroupField
    {
        get => _selectedGroupField;
        set => SetProperty(ref _selectedGroupField, value);
    }

    public QueryAggregate? SelectedAggregate
    {
        get => _selectedAggregate;
        set => SetProperty(ref _selectedAggregate, value);
    }

    public FieldMetadata? SelectedAggregateField
    {
        get => _selectedAggregateField;
        set => SetProperty(ref _selectedAggregateField, value);
    }

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

    public new bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetProperty(ref _validationMessage, value);
    }

    public QueryResult? QueryResult
    {
        get => _queryResult;
        set
        {
            if (SetProperty(ref _queryResult, value))
            {
                ColumnsGenerated = false;
                OnPropertyChanged(nameof(ColumnsGenerated));
            }
        }
    }

    public bool ColumnsGenerated
    {
        get => _columnsGenerated;
        set => SetProperty(ref _columnsGenerated, value);
    }

    private bool _columnsGenerated;
    private bool _hasResultLimitWarning;
    private const int MaxResultLimit = 10000;

    public bool HasResultLimitWarning
    {
        get => _hasResultLimitWarning;
        set => SetProperty(ref _hasResultLimitWarning, value);
    }

    public void GenerateColumns()
    {
        ColumnsGenerated = true;
    }

    [RelayCommand]
    private void AddColumn()
    {
        if (SelectedAvailableColumn == null) return;

        if (!SelectedColumns.Contains(SelectedAvailableColumn))
        {
            SelectedColumns.Add(SelectedAvailableColumn);
            _queryDefinition.SelectedColumns.Add(SelectedAvailableColumn.FieldId);
        }
    }

    [RelayCommand]
    private void RemoveColumn()
    {
        if (SelectedSelectedColumn == null) return;

        SelectedColumns.Remove(SelectedSelectedColumn);
        _queryDefinition.SelectedColumns.Remove(SelectedSelectedColumn.FieldId);
    }

    [RelayCommand]
    private void MoveColumnUp()
    {
        if (SelectedSelectedColumn == null) return;

        var index = SelectedColumns.IndexOf(SelectedSelectedColumn);
        if (index > 0)
        {
            var item = SelectedColumns[index];
            SelectedColumns.RemoveAt(index);
            SelectedColumns.Insert(index - 1, item);
            
            var fieldId = _queryDefinition.SelectedColumns[index];
            _queryDefinition.SelectedColumns.RemoveAt(index);
            _queryDefinition.SelectedColumns.Insert(index - 1, fieldId);
        }
    }

    [RelayCommand]
    private void MoveColumnDown()
    {
        if (SelectedSelectedColumn == null) return;

        var index = SelectedColumns.IndexOf(SelectedSelectedColumn);
        if (index < SelectedColumns.Count - 1)
        {
            var item = SelectedColumns[index];
            SelectedColumns.RemoveAt(index);
            SelectedColumns.Insert(index + 1, item);
            
            var fieldId = _queryDefinition.SelectedColumns[index];
            _queryDefinition.SelectedColumns.RemoveAt(index);
            _queryDefinition.SelectedColumns.Insert(index + 1, fieldId);
        }
    }

    [RelayCommand]
    private void AddFilter()
    {
        if (SelectedModule == null) return;

        var filter = new QueryFilter
        {
            FieldId = SelectedModule.Fields.FirstOrDefault()?.FieldId ?? string.Empty,
            Operator = FilterOperator.Equals
        };

        Filters.Add(filter);
        _queryDefinition.Filters.Add(filter);
    }

    [RelayCommand]
    private void RemoveFilter()
    {
        if (SelectedFilter == null) return;

        Filters.Remove(SelectedFilter);
        _queryDefinition.Filters.Remove(SelectedFilter);
    }

    [RelayCommand]
    private void AddSort()
    {
        if (SelectedModule == null) return;

        var sort = new QuerySort
        {
            FieldId = SelectedModule.Fields.FirstOrDefault()?.FieldId ?? string.Empty,
            Ascending = true,
            Priority = Sorts.Count
        };

        Sorts.Add(sort);
        _queryDefinition.Sorts.Add(sort);
    }

    [RelayCommand]
    private void RemoveSort()
    {
        if (SelectedSort == null) return;

        Sorts.Remove(SelectedSort);
        _queryDefinition.Sorts.Remove(SelectedSort);
        UpdateSortPriorities();
    }

    [RelayCommand]
    private void MoveSortUp()
    {
        if (SelectedSort == null) return;

        var index = Sorts.IndexOf(SelectedSort);
        if (index > 0)
        {
            var item = Sorts[index];
            Sorts.RemoveAt(index);
            Sorts.Insert(index - 1, item);
            
            var sortItem = _queryDefinition.Sorts[index];
            _queryDefinition.Sorts.RemoveAt(index);
            _queryDefinition.Sorts.Insert(index - 1, sortItem);
            
            UpdateSortPriorities();
        }
    }

    [RelayCommand]
    private void MoveSortDown()
    {
        if (SelectedSort == null) return;

        var index = Sorts.IndexOf(SelectedSort);
        if (index < Sorts.Count - 1)
        {
            var item = Sorts[index];
            Sorts.RemoveAt(index);
            Sorts.Insert(index + 1, item);
            
            var sortItem = _queryDefinition.Sorts[index];
            _queryDefinition.Sorts.RemoveAt(index);
            _queryDefinition.Sorts.Insert(index + 1, sortItem);
            
            UpdateSortPriorities();
        }
    }

    private void UpdateSortPriorities()
    {
        for (int i = 0; i < Sorts.Count; i++)
        {
            Sorts[i].Priority = i;
            _queryDefinition.Sorts[i].Priority = i;
        }
    }

    [RelayCommand]
    private void AddAggregate()
    {
        if (SelectedModule == null) return;

        var aggregate = new QueryAggregate
        {
            FieldId = SelectedModule.Fields.FirstOrDefault(f => f.DataType == FieldDataType.Number)?.FieldId ?? string.Empty,
            AggregateType = AggregateType.Sum,
            DisplayName = "Sum"
        };

        Aggregates.Add(aggregate);
        _queryDefinition.Aggregates.Add(aggregate);
    }

    [RelayCommand]
    private void RemoveAggregate()
    {
        if (SelectedAggregate == null) return;

        Aggregates.Remove(SelectedAggregate);
        _queryDefinition.Aggregates.Remove(SelectedAggregate);
    }

    [RelayCommand]
    private async Task ExecuteQueryAsync()
    {
        if (SelectedModule == null)
        {
            ValidationMessage = "Please select a module.";
            return;
        }

        _queryDefinition.GroupByFieldId = SelectedGroupField?.FieldId;

        var errors = _queryDefinition.Validate();
        if (errors.Any())
        {
            ValidationMessage = string.Join("\n", errors);
            return;
        }

        ValidationMessage = string.Empty;
        IsBusy = true;

        try
        {
            _searchCancellationTokenSource?.Cancel();
            _searchCancellationTokenSource = new CancellationTokenSource();

            QueryResult = await _queryEngine.ExecuteQueryAsync(
                _queryDefinition,
                SearchText,
                _searchCancellationTokenSource.Token).ConfigureAwait(false);

            ResultItems.Clear();
            foreach (var item in QueryResult.Items)
            {
                ResultItems.Add(item);
            }

            HasResultLimitWarning = QueryResult.TotalCount >= MaxResultLimit;

            await _notificationService.ShowSuccessAsync("Success", $"Query executed successfully. {QueryResult.TotalCount} records returned.").ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await _notificationService.ShowInformationAsync("Cancelled", "Query execution was cancelled.").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _notificationService.ShowErrorAsync("Error", $"Error executing query: {ex.Message}").ConfigureAwait(false);
            ValidationMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        Filters.Clear();
        _queryDefinition.Filters.Clear();
    }

    [RelayCommand]
    private void ClearSorts()
    {
        Sorts.Clear();
        _queryDefinition.Sorts.Clear();
    }

    [RelayCommand]
    private void ClearAggregates()
    {
        Aggregates.Clear();
        _queryDefinition.Aggregates.Clear();
    }

    [RelayCommand]
    private void ClearGrouping()
    {
        SelectedGroupField = null;
        _queryDefinition.GroupByFieldId = null;
    }

    [RelayCommand]
    private void ResetQuery()
    {
        SelectedModule = null;
        SelectedColumns.Clear();
        AvailableColumns.Clear();
        Filters.Clear();
        Sorts.Clear();
        Aggregates.Clear();
        SelectedGroupField = null;
        ResultItems.Clear();
        QueryResult = null;
        ValidationMessage = string.Empty;
        SearchText = string.Empty;
        _queryDefinition = new QueryDefinition();
    }

    private async Task DebouncedSearchAsync()
    {
        _searchCancellationTokenSource?.Cancel();
        _searchCancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(300, _searchCancellationTokenSource.Token);
            await ExecuteQueryAsync();
        }
        catch (TaskCanceledException)
        {
        }
    }

    [RelayCommand]
    private async Task ExportToExcelAsync()
    {
        if (QueryResult == null || SelectedModule == null)
        {
            await _notificationService.ShowWarningAsync("Warning", "No query results to export.").ConfigureAwait(false);
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Excel Files|*.xlsx",
            DefaultExt = "xlsx",
            FileName = $"Query_Result_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            IsBusy = true;
            try
            {
                await _excelExporter.ExportToExcelAsync(
                    QueryResult,
                    SelectedModule,
                    _queryDefinition,
                    saveFileDialog.FileName).ConfigureAwait(false);

                await _notificationService.ShowSuccessAsync("Success", "Excel export completed successfully.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Error", $"Error exporting to Excel: {ex.Message}").ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    [RelayCommand]
    private async Task ExportToPdfAsync()
    {
        if (QueryResult == null || SelectedModule == null)
        {
            await _notificationService.ShowWarningAsync("Warning", "No query results to export.").ConfigureAwait(false);
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "PDF Files|*.pdf",
            DefaultExt = "pdf",
            FileName = $"Query_Result_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            IsBusy = true;
            try
            {
                await _pdfExporter.ExportToPdfAsync(
                    QueryResult,
                    SelectedModule,
                    _queryDefinition,
                    saveFileDialog.FileName).ConfigureAwait(false);

                await _notificationService.ShowSuccessAsync("Success", "PDF export completed successfully.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Error", $"Error exporting to PDF: {ex.Message}").ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    [RelayCommand]
    private async Task ExportToCsvAsync()
    {
        if (QueryResult == null || SelectedModule == null)
        {
            await _notificationService.ShowWarningAsync("Warning", "No query results to export.").ConfigureAwait(false);
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = "CSV Files|*.csv",
            DefaultExt = "csv",
            FileName = $"Query_Result_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            IsBusy = true;
            try
            {
                await _csvExporter.ExportToCsvAsync(
                    QueryResult,
                    SelectedModule,
                    _queryDefinition,
                    saveFileDialog.FileName).ConfigureAwait(false);

                await _notificationService.ShowSuccessAsync("Success", "CSV export completed successfully.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _notificationService.ShowErrorAsync("Error", $"Error exporting to CSV: {ex.Message}").ConfigureAwait(false);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    [RelayCommand]
    private async Task PrintAsync()
    {
        if (QueryResult == null || SelectedModule == null)
        {
            await _notificationService.ShowWarningAsync("Warning", "No query results to print.").ConfigureAwait(false);
            return;
        }

        var tempPdfPath = Path.Combine(Path.GetTempPath(), $"Query_Result_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

        IsBusy = true;
        try
        {
            await _pdfExporter.ExportToPdfAsync(
                QueryResult,
                SelectedModule,
                _queryDefinition,
                tempPdfPath).ConfigureAwait(false);

            var printProcess = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempPdfPath,
                    UseShellExecute = true,
                    Verb = "print"
                }
            };

            printProcess.Start();
            await _notificationService.ShowSuccessAsync("Success", "Print command sent.").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _notificationService.ShowErrorAsync("Error", $"Error printing: {ex.Message}").ConfigureAwait(false);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
