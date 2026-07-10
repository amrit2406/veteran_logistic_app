using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.ViewModels;
using veteran_logistic.Navigation;

namespace veteran_logistic.Shell;

/// <summary>
/// Shell view model that exposes the current ViewModel provided by navigation service.
/// </summary>
public sealed class ShellViewModel : ObservableObject
{
    private readonly PlaceholderViewModel _placeholder = new();
    private readonly ILogoutService _logoutService;
    private object? _currentViewModel;

    private void OnCurrentViewModelChanged(object? vm)
    {
        CurrentViewModel = ResolveShellContent(vm) ?? _placeholder;
    }

    private static object? ResolveShellContent(object? vm) => vm switch
    {
        null or ShellViewModel or LoginViewModel => null,
        _ => vm
    };

    /// <summary>
    /// Current active view model displayed in the shell.
    /// </summary>
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    /// <summary>
    /// Command to logout the current user and return to the login screen.
    /// </summary>
    public IAsyncRelayCommand LogoutCommand { get; }

    /// <summary>
    /// Command to navigate to the Users screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToUsersCommand { get; }

    /// <summary>
    /// Command to navigate to the Roles screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToRolesCommand { get; }

    /// <summary>
    /// Command to navigate to the Permission Matrix screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToPermissionMatrixCommand { get; }

    /// <summary>
    /// Command to navigate to the Financial Years screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToFinancialYearsCommand { get; }

    /// <summary>
    /// Command to navigate to the Companies screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToCompaniesCommand { get; }

    /// <summary>
    /// Command to navigate to the Customers screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToCustomersCommand { get; }

    public ShellViewModel(INavigationService navigationService, ILogoutService logoutService)
    {
        if (navigationService is null) throw new ArgumentNullException(nameof(navigationService));
        if (logoutService is null) throw new ArgumentNullException(nameof(logoutService));

        _logoutService = logoutService;
        LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
        NavigateToUsersCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Users.ViewModels.UsersViewModel>());
        NavigateToRolesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Roles.ViewModels.RolesViewModel>());
        NavigateToPermissionMatrixCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Permissions.ViewModels.PermissionMatrixViewModel>());
        NavigateToFinancialYearsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.FinancialYears.ViewModels.FinancialYearsViewModel>());
        NavigateToCompaniesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Companies.ViewModels.CompaniesViewModel>());
        NavigateToCustomersCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Customers.ViewModels.CustomersViewModel>());
        navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _currentViewModel = ResolveShellContent(navigationService.CurrentViewModel) ?? _placeholder;
    }

    private async Task ExecuteLogoutAsync()
    {
        await _logoutService.LogoutAsync();
    }
}

/// <summary>
/// Temporary placeholder ViewModel shown before authentication or initial module loads.
/// </summary>
public sealed class PlaceholderViewModel
{
    public string Title { get; } = "Veteran Logistics";
    public string Subtitle { get; } = "Enterprise Logistics ERP";
    public string Message { get; } = "Application initialized successfully. Authentication module will appear here.";
}
