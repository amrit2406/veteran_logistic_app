using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.FinancialYears.ViewModels;

/// <summary>
/// ViewModel for the Add Financial Year screen.
/// </summary>
public sealed partial class AddFinancialYearViewModel : ViewModelBase
{
    private readonly IFinancialYearCommandService _financialYearCommandService;
    private readonly INavigationService _navigationService;
    private string _name = string.Empty;
    private DateTime _startDate = DateTime.Today;
    private DateTime _endDate = DateTime.Today.AddYears(1);
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddFinancialYearViewModel"/> class.
    /// </summary>
    /// <param name="financialYearCommandService">The financial year command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddFinancialYearViewModel(IFinancialYearCommandService financialYearCommandService, INavigationService navigationService)
    {
        _financialYearCommandService = financialYearCommandService ?? throw new ArgumentNullException(nameof(financialYearCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Financial Year";
    }

    /// <summary>
    /// Gets or sets the financial year name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Gets or sets the start date of the financial year.
    /// </summary>
    public DateTime StartDate
    {
        get => _startDate;
        set => SetProperty(ref _startDate, value);
    }

    /// <summary>
    /// Gets or sets the end date of the financial year.
    /// </summary>
    public DateTime EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value);
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
    /// Command to save the financial year.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateFinancialYearRequest
        {
            Name = Name,
            StartDate = StartDate,
            EndDate = EndDate
        };

        SetBusy("Creating financial year...");
        var result = await _financialYearCommandService.CreateFinancialYearAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create financial year.";
        }
    }

    /// <summary>
    /// Command to cancel and navigate back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync().ConfigureAwait(false);
    }
}
