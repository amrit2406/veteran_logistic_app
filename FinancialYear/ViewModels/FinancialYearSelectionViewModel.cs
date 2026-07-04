using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.FinancialYear.ViewModels;

/// <summary>
/// ViewModel for the Financial Year Selection screen.
/// </summary>
public sealed partial class FinancialYearSelectionViewModel : ViewModelBase
{
    private readonly IFinancialYearService _financialYearService;
    private readonly IFinancialYearContext _financialYearContext;
    private readonly IApplicationContext _applicationContext;
    private readonly INavigationService _navigationService;

    private veteran_logistic.FinancialYear.Models.FinancialYear? _selectedFinancialYear;
    private string? _errorMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearSelectionViewModel"/> class.
    /// </summary>
    public FinancialYearSelectionViewModel(
        IFinancialYearService financialYearService,
        IFinancialYearContext financialYearContext,
        IApplicationContext applicationContext,
        INavigationService navigationService)
    {
        _financialYearService = financialYearService ?? throw new ArgumentNullException(nameof(financialYearService));
        _financialYearContext = financialYearContext ?? throw new ArgumentNullException(nameof(financialYearContext));
        _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        FinancialYears = new ObservableCollection<veteran_logistic.FinancialYear.Models.FinancialYear>();
    }

    /// <summary>
    /// Gets the collection of available financial years.
    /// </summary>
    public ObservableCollection<veteran_logistic.FinancialYear.Models.FinancialYear> FinancialYears { get; }

    /// <summary>
    /// Gets or sets the selected financial year.
    /// </summary>
    public veteran_logistic.FinancialYear.Models.FinancialYear? SelectedFinancialYear
    {
        get => _selectedFinancialYear;
        set
        {
            if (SetProperty(ref _selectedFinancialYear, value))
            {
                ContinueCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    /// <inheritdoc />
    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        try
        {
            SetBusy("Loading financial years...");
            ErrorMessage = null;

            var years = await _financialYearService.GetAvailableFinancialYearsAsync(cancellationToken);
            
            FinancialYears.Clear();
            foreach (var year in years)
            {
                FinancialYears.Add(year);
            }

            // Optionally pre-select the first one or current year
            if (FinancialYears.Any())
            {
                SelectedFinancialYear = FinancialYears.OrderByDescending(y => y.StartDate).First();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load financial years: {ex.Message}";
        }
        finally
        {
            ClearBusy();
            await base.InitializeAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Command to continue to the Shell with the selected financial year.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanContinue))]
    private async Task ContinueAsync()
    {
        if (SelectedFinancialYear == null || IsBusy)
        {
            return;
        }

        try
        {
            SetBusy("Applying selection...");
            ErrorMessage = null;

            var result = await _financialYearService.SelectFinancialYearAsync(SelectedFinancialYear.Id);

            if (result.IsSuccess)
            {
                // Store in context
                _financialYearContext.SetFinancialYear(result.SelectedFinancialYear!);
                
                // Navigate to Shell
                await _navigationService.NavigateAsync<Shell.ShellViewModel>();
            }
            else
            {
                ErrorMessage = result.ErrorMessage ?? "Failed to select financial year.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            ClearBusy();
        }
    }

    private bool CanContinue()
    {
        return SelectedFinancialYear != null && !IsBusy;
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    [RelayCommand]
    private void Exit()
    {
        System.Windows.Application.Current.Shutdown();
    }
}
