using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.FinancialYears.ViewModels;

/// <summary>
/// ViewModel for the Financial Years listing screen.
/// </summary>
public sealed partial class FinancialYearsViewModel : ViewModelBase
{
    private readonly IFinancialYearQueryService _financialYearQueryService;
    private readonly INavigationService _navigationService;
    private FinancialYearListItem? _selectedFinancialYear;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearsViewModel"/> class.
    /// </summary>
    /// <param name="financialYearQueryService">The financial year query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public FinancialYearsViewModel(IFinancialYearQueryService financialYearQueryService, INavigationService navigationService)
    {
        _financialYearQueryService = financialYearQueryService ?? throw new ArgumentNullException(nameof(financialYearQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Financial Years";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadFinancialYearsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadFinancialYearsAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of financial years to display.
    /// </summary>
    public ObservableCollection<FinancialYearListItem> FinancialYears { get; } = new();

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Gets or sets the selected financial year.
    /// </summary>
    public FinancialYearListItem? SelectedFinancialYear
    {
        get => _selectedFinancialYear;
        set => SetProperty(ref _selectedFinancialYear, value);
    }

    /// <summary>
    /// Command to refresh the financial year list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadFinancialYearsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Financial Year screen.
    /// </summary>
    [RelayCommand]
    private async Task AddFinancialYearAsync()
    {
        await _navigationService.NavigateAsync<AddFinancialYearViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Financial Year screen.
    /// </summary>
    [RelayCommand]
    private async Task EditFinancialYearAsync()
    {
        if (SelectedFinancialYear is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["FinancialYearId"] = SelectedFinancialYear.Id
        };

        await _navigationService.NavigateAsync<EditFinancialYearViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Loads all financial years.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading financial years...");
        var financialYears = await _financialYearQueryService.GetAllFinancialYearsAsync(cancellationToken);
        UpdateFinancialYears(financialYears);
        ClearBusy();
    }

    /// <summary>
    /// Updates the financial years collection on the UI thread.
    /// </summary>
    /// <param name="financialYears">The financial years to update.</param>
    private void UpdateFinancialYears(IEnumerable<FinancialYearListItem> financialYears)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateFinancialYearsInternal(financialYears);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateFinancialYearsInternal(financialYears));
        }
    }

    /// <summary>
    /// Updates the financial years collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="financialYears">The financial years to update.</param>
    private void UpdateFinancialYearsInternal(IEnumerable<FinancialYearListItem> financialYears)
    {
        FinancialYears.Clear();
        foreach (var financialYear in financialYears)
        {
            FinancialYears.Add(financialYear);
        }
    }
}
