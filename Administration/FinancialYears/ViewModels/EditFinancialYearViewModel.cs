using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.FinancialYears.ViewModels;

/// <summary>
/// ViewModel for the Edit Financial Year screen.
/// </summary>
public sealed partial class EditFinancialYearViewModel : ViewModelBase, INavigationAware
{
    private readonly IFinancialYearCommandService _financialYearCommandService;
    private readonly IFinancialYearQueryService _financialYearQueryService;
    private readonly INavigationService _navigationService;
    private int _financialYearId;
    private string _name = string.Empty;
    private DateTime _startDate;
    private DateTime _endDate;
    private bool _isCurrent;
    private bool _isClosed;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditFinancialYearViewModel"/> class.
    /// </summary>
    /// <param name="financialYearCommandService">The financial year command service.</param>
    /// <param name="financialYearQueryService">The financial year query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditFinancialYearViewModel(
        IFinancialYearCommandService financialYearCommandService,
        IFinancialYearQueryService financialYearQueryService,
        INavigationService navigationService)
    {
        _financialYearCommandService = financialYearCommandService ?? throw new ArgumentNullException(nameof(financialYearCommandService));
        _financialYearQueryService = financialYearQueryService ?? throw new ArgumentNullException(nameof(financialYearQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Financial Year";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("FinancialYearId", out var financialYearId))
        {
            _financialYearId = financialYearId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadFinancialYearAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
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
    /// Gets or sets whether this is the current financial year.
    /// </summary>
    public bool IsCurrent
    {
        get => _isCurrent;
        set => SetProperty(ref _isCurrent, value);
    }

    /// <summary>
    /// Gets or sets whether the financial year is closed.
    /// </summary>
    public bool IsClosed
    {
        get => _isClosed;
        set => SetProperty(ref _isClosed, value);
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
    /// Loads the financial year data for editing.
    /// </summary>
    private async Task LoadFinancialYearAsync(CancellationToken cancellationToken = default)
    {
        if (_financialYearId == 0)
        {
            ValidationError = "Financial Year ID is required.";
            return;
        }

        SetBusy("Loading financial year...");
        var financialYear = await _financialYearQueryService.GetFinancialYearForEditAsync(_financialYearId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (financialYear is null)
        {
            ValidationError = "Financial year not found.";
            return;
        }

        Name = financialYear.Name;
        StartDate = financialYear.StartDate;
        EndDate = financialYear.EndDate;
        IsCurrent = financialYear.IsCurrent;
        IsClosed = financialYear.IsClosed;
    }

    /// <summary>
    /// Command to save the financial year.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateFinancialYearRequest
        {
            Id = _financialYearId,
            Name = Name,
            StartDate = StartDate,
            EndDate = EndDate,
            IsCurrent = IsCurrent,
            IsClosed = IsClosed
        };

        SetBusy("Updating financial year...");
        var result = await _financialYearCommandService.UpdateFinancialYearAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update financial year.";
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
