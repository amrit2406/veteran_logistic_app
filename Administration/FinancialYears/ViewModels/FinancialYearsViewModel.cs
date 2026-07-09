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
    private readonly IFinancialYearCommandService _financialYearCommandService;
    private readonly INavigationService _navigationService;
    private FinancialYearListItem? _selectedFinancialYear;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearsViewModel"/> class.
    /// </summary>
    /// <param name="financialYearQueryService">The financial year query service.</param>
    /// <param name="financialYearCommandService">The financial year command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public FinancialYearsViewModel(
        IFinancialYearQueryService financialYearQueryService,
        IFinancialYearCommandService financialYearCommandService,
        INavigationService navigationService)
    {
        _financialYearQueryService = financialYearQueryService ?? throw new ArgumentNullException(nameof(financialYearQueryService));
        _financialYearCommandService = financialYearCommandService ?? throw new ArgumentNullException(nameof(financialYearCommandService));
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
        set
        {
            if (SetProperty(ref _selectedFinancialYear, value))
            {
                EditFinancialYearCommand.NotifyCanExecuteChanged();
                SetCurrentFinancialYearCommand.NotifyCanExecuteChanged();
                CloseFinancialYearCommand.NotifyCanExecuteChanged();
                DeleteFinancialYearCommand.NotifyCanExecuteChanged();
            }
        }
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
    [RelayCommand(CanExecute = nameof(CanExecuteEditFinancialYear))]
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

    private bool CanExecuteEditFinancialYear()
    {
        return SelectedFinancialYear is not null;
    }

    /// <summary>
    /// Command to set the selected financial year as current.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteSetCurrentFinancialYear))]
    private async Task SetCurrentFinancialYearAsync()
    {
        if (SelectedFinancialYear is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            $"Are you sure you want to set '{SelectedFinancialYear.Name}' as the current financial year?",
            "Set Current Financial Year",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new SetCurrentFinancialYearRequest
        {
            FinancialYearId = SelectedFinancialYear.Id
        };

        SetBusy("Setting current financial year...");
        var result = await _financialYearCommandService.SetCurrentFinancialYearAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadFinancialYearsAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to set current financial year.";
        }
    }

    private bool CanExecuteSetCurrentFinancialYear()
    {
        return SelectedFinancialYear is not null && !SelectedFinancialYear.IsCurrent;
    }

    /// <summary>
    /// Command to close the selected financial year.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCloseFinancialYear))]
    private async Task CloseFinancialYearAsync()
    {
        if (SelectedFinancialYear is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to close this Financial Year?\n\nThis action cannot be undone.",
            "Close Financial Year",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new CloseFinancialYearRequest
        {
            FinancialYearId = SelectedFinancialYear.Id
        };

        SetBusy("Closing financial year...");
        var result = await _financialYearCommandService.CloseFinancialYearAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadFinancialYearsAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to close financial year.";
        }
    }

    private bool CanExecuteCloseFinancialYear()
    {
        return SelectedFinancialYear is not null && !SelectedFinancialYear.IsClosed;
    }

    /// <summary>
    /// Command to delete the selected financial year.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteDeleteFinancialYear))]
    private async Task DeleteFinancialYearAsync()
    {
        if (SelectedFinancialYear is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this Financial Year?\n\nThis action cannot be undone.",
            "Delete Financial Year",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteFinancialYearRequest
        {
            FinancialYearId = SelectedFinancialYear.Id
        };

        SetBusy("Deleting financial year...");
        var result = await _financialYearCommandService.DeleteFinancialYearAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadFinancialYearsAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete financial year.";
        }
    }

    private bool CanExecuteDeleteFinancialYear()
    {
        return SelectedFinancialYear is not null;
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
