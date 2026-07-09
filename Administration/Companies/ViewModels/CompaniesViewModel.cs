using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Administration.Companies.Contracts;
using veteran_logistic.Administration.Companies.Models;
using veteran_logistic.MVVM;

namespace veteran_logistic.Administration.Companies.ViewModels;

/// <summary>
/// ViewModel for the Companies listing screen.
/// </summary>
public sealed partial class CompaniesViewModel : ViewModelBase
{
    private readonly ICompanyQueryService _companyQueryService;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompaniesViewModel"/> class.
    /// </summary>
    /// <param name="companyQueryService">The company query service.</param>
    public CompaniesViewModel(ICompanyQueryService companyQueryService)
    {
        _companyQueryService = companyQueryService ?? throw new ArgumentNullException(nameof(companyQueryService));

        Title = "Companies";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadCompaniesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadCompaniesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of companies to display.
    /// </summary>
    public ObservableCollection<CompanyListItem> Companies { get; } = new();

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Command to refresh the company list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadCompaniesAsync();
    }

    /// <summary>
    /// Loads all companies.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadCompaniesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading companies...");
        var companies = await _companyQueryService.GetAllCompaniesAsync(cancellationToken);
        UpdateCompanies(companies);
        ClearBusy();
    }

    /// <summary>
    /// Updates the companies collection on the UI thread.
    /// </summary>
    /// <param name="companies">The companies to update.</param>
    private void UpdateCompanies(IEnumerable<CompanyListItem> companies)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateCompaniesInternal(companies);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateCompaniesInternal(companies));
        }
    }

    /// <summary>
    /// Updates the companies collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="companies">The companies to update.</param>
    private void UpdateCompaniesInternal(IEnumerable<CompanyListItem> companies)
    {
        Companies.Clear();
        foreach (var company in companies)
        {
            Companies.Add(company);
        }
    }
}
