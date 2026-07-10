using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Companies.ViewModels;

/// <summary>
/// ViewModel for the Companies listing screen.
/// </summary>
public sealed partial class CompaniesViewModel : ViewModelBase
{
    private readonly ICompanyQueryService _companyQueryService;
    private readonly ICompanyCommandService _companyCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private CompanyListItem? _selectedCompany;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompaniesViewModel"/> class.
    /// </summary>
    /// <param name="companyQueryService">The company query service.</param>
    /// <param name="companyCommandService">The company command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public CompaniesViewModel(ICompanyQueryService companyQueryService, ICompanyCommandService companyCommandService, INavigationService navigationService)
    {
        _companyQueryService = companyQueryService ?? throw new ArgumentNullException(nameof(companyQueryService));
        _companyCommandService = companyCommandService ?? throw new ArgumentNullException(nameof(companyCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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
    /// Gets or sets the selected company.
    /// </summary>
    public CompanyListItem? SelectedCompany
    {
        get => _selectedCompany;
        set
        {
            if (SetProperty(ref _selectedCompany, value))
            {
                EditCompanyCommand.NotifyCanExecuteChanged();
                ActivateCompanyCommand.NotifyCanExecuteChanged();
                DeactivateCompanyCommand.NotifyCanExecuteChanged();
                DeleteCompanyCommand.NotifyCanExecuteChanged();
            }
        }
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
    /// Command to refresh the company list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadCompaniesAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Company screen.
    /// </summary>
    [RelayCommand]
    private async Task AddCompanyAsync()
    {
        await _navigationService.NavigateAsync<AddCompanyViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Company screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCompanyCommand))]
    private async Task EditCompanyAsync()
    {
        if (SelectedCompany is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["CompanyId"] = SelectedCompany.Id
        };

        await _navigationService.NavigateAsync<EditCompanyViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected company.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCompanyCommand))]
    private async Task ActivateCompanyAsync()
    {
        if (SelectedCompany is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateCompanyStatusRequest
        {
            CompanyId = SelectedCompany.Id,
            IsActive = true
        };

        SetBusy("Activating company...");
        var result = await _companyCommandService.UpdateCompanyStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleCompanyStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate company.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected company.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCompanyCommand))]
    private async Task DeactivateCompanyAsync()
    {
        if (SelectedCompany is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateCompanyStatusRequest
        {
            CompanyId = SelectedCompany.Id,
            IsActive = false
        };

        SetBusy("Deactivating company...");
        var result = await _companyCommandService.UpdateCompanyStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleCompanyStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate company.";
        }
    }

    private async Task HandleCompanyStatusUpdateSuccess()
    {
        await LoadCompaniesAsync();
        SelectedCompany = null;
        ActivateCompanyCommand.NotifyCanExecuteChanged();
        DeactivateCompanyCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected company.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCompanyCommand))]
    private async Task DeleteCompanyAsync()
    {
        if (SelectedCompany is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this company?\n\nThis action hides the company from the application.",
            "Delete Company",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteCompanyRequest
        {
            CompanyId = SelectedCompany.Id
        };

        SetBusy("Deleting company...");
        var result = await _companyCommandService.DeleteCompanyAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadCompaniesAsync();
            SelectedCompany = null;
            DeleteCompanyCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete company.";
        }
    }

    private bool CanExecuteCompanyCommand()
    {
        return SelectedCompany is not null;
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
    /// Debounced search to prevent excessive database queries.
    /// </summary>
    private async Task DebouncedSearchAsync()
    {
        // Cancel and dispose previous search if still running
        var cts = _searchCancellationTokenSource;
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }

        _searchCancellationTokenSource = new CancellationTokenSource();
        var token = _searchCancellationTokenSource.Token;

        try
        {
            // Wait 300ms to allow user to finish typing
            await Task.Delay(300, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            await SearchCompaniesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches companies based on the current search text.
    /// </summary>
    private async Task SearchCompaniesAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching companies...");
        var companies = await _companyQueryService.SearchCompaniesAsync(SearchText, cancellationToken);
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
