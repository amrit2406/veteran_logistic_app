using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.ViewModels;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;
using veteran_logistic.Navigation;

namespace veteran_logistic.Shell;

/// <summary>
/// Shell view model that exposes the current ViewModel provided by navigation service.
/// </summary>
public sealed class ShellViewModel : ObservableObject
{
    private readonly PlaceholderViewModel _placeholder = new();
    private readonly ILogoutService _logoutService;
    private readonly INavigationService _navigationService;
    private readonly IPermissionAuthorizationService _permissionAuthorizationService;
    private object? _currentViewModel;

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

    /// <summary>
    /// Command to navigate to the Vendors screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToVendorsCommand { get; }

    /// <summary>
    /// Command to navigate to the Sources screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToSourcesCommand { get; }

    /// <summary>
    /// Command to navigate to the Destinations screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToDestinationsCommand { get; }

    /// <summary>
    /// Command to navigate to the Materials screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToMaterialsCommand { get; }

    /// <summary>
    /// Command to navigate to the Fuel Pumps screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToFuelPumpsCommand { get; }

    /// <summary>
    /// Command to navigate to the HSD Rates screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToHsdRatesCommand { get; }

    /// <summary>
    /// Command to navigate to the Payment Locations screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToPaymentLocationsCommand { get; }

    /// <summary>
    /// Command to navigate to the Vehicle Owners screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToVehicleOwnersCommand { get; }

    /// <summary>
    /// Command to navigate to the Vehicles screen.
    /// </summary>
    public IAsyncRelayCommand NavigateToVehiclesCommand { get; }

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    public ShellViewModel(INavigationService navigationService, ILogoutService logoutService, IPermissionAuthorizationService permissionAuthorizationService)
    {
        if (navigationService is null) throw new ArgumentNullException(nameof(navigationService));
        if (logoutService is null) throw new ArgumentNullException(nameof(logoutService));
        if (permissionAuthorizationService is null) throw new ArgumentNullException(nameof(permissionAuthorizationService));

        _navigationService = navigationService;
        _logoutService = logoutService;
        _permissionAuthorizationService = permissionAuthorizationService;
        LogoutCommand = new AsyncRelayCommand(ExecuteLogoutAsync);
        NavigateToUsersCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Users.ViewModels.UsersViewModel>(), () => CanNavigateToUsers());
        NavigateToRolesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Roles.ViewModels.RolesViewModel>(), () => CanNavigateToRoles());
        NavigateToPermissionMatrixCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.Permissions.ViewModels.PermissionMatrixViewModel>(), () => CanNavigateToPermissionMatrix());
        NavigateToFinancialYearsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Administration.FinancialYears.ViewModels.FinancialYearsViewModel>(), () => CanNavigateToFinancialYears());
        NavigateToCompaniesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Companies.ViewModels.CompaniesViewModel>(), () => CanNavigateToCompanies());
        NavigateToCustomersCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Customers.ViewModels.CustomersViewModel>(), () => CanNavigateToCustomers());
        NavigateToVendorsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Vendors.ViewModels.VendorsViewModel>(), () => CanNavigateToVendors());
        NavigateToSourcesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Sources.ViewModels.SourcesViewModel>(), () => CanNavigateToSources());
        NavigateToDestinationsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Destinations.ViewModels.DestinationsViewModel>(), () => CanNavigateToDestinations());
        NavigateToMaterialsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Materials.ViewModels.MaterialsViewModel>(), () => CanNavigateToMaterials());
        NavigateToFuelPumpsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.FuelPumps.ViewModels.FuelPumpsViewModel>(), () => CanNavigateToFuelPumps());
        NavigateToHsdRatesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.HsdRates.ViewModels.HsdRatesViewModel>(), () => CanNavigateToHsdRates());
        NavigateToPaymentLocationsCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.PaymentLocations.ViewModels.PaymentLocationsViewModel>(), () => CanNavigateToPaymentLocations());
        NavigateToVehicleOwnersCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.VehicleOwners.ViewModels.VehicleOwnersViewModel>(), () => CanNavigateToVehicleOwners());
        NavigateToVehiclesCommand = new AsyncRelayCommand(() => navigationService.NavigateAsync<veteran_logistic.Masters.Vehicles.ViewModels.VehiclesViewModel>(), () => CanNavigateToVehicles());
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync);
        navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _currentViewModel = ResolveShellContent(navigationService.CurrentViewModel) ?? _placeholder;
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        OnPropertyChanged(nameof(CanGoBack));
    }

    private void OnCurrentViewModelChanged(object? vm)
    {
        CurrentViewModel = ResolveShellContent(vm) ?? _placeholder;
        OnPropertyChanged(nameof(CanGoBack));
        
        // Refresh command can execute states when navigation changes
        ((AsyncRelayCommand)NavigateToUsersCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToRolesCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToPermissionMatrixCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToFinancialYearsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToCompaniesCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToCustomersCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToVendorsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToSourcesCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToDestinationsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToMaterialsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToFuelPumpsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToHsdRatesCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToPaymentLocationsCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToVehicleOwnersCommand).NotifyCanExecuteChanged();
        ((AsyncRelayCommand)NavigateToVehiclesCommand).NotifyCanExecuteChanged();
    }

    private bool CanNavigateToUsers()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewUsers);
    }

    private bool CanNavigateToRoles()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewRoles);
    }

    private bool CanNavigateToPermissionMatrix()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewPermissionMatrix);
    }

    private bool CanNavigateToFinancialYears()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewFinancialYears);
    }

    private bool CanNavigateToCompanies()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewCompanies);
    }

    private bool CanNavigateToCustomers()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewCustomers);
    }

    private bool CanNavigateToVendors()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewVendors);
    }

    private bool CanNavigateToSources()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewSources);
    }

    private bool CanNavigateToDestinations()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewDestinations);
    }

    private bool CanNavigateToMaterials()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewMaterials);
    }

    private bool CanNavigateToFuelPumps()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewFuelPumps);
    }

    private bool CanNavigateToHsdRates()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewHsdRates);
    }

    private bool CanNavigateToPaymentLocations()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewPaymentLocations);
    }

    private bool CanNavigateToVehicleOwners()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewVehicleOwners);
    }

    private bool CanNavigateToVehicles()
    {
        return _permissionAuthorizationService.HasPermission(ApplicationPermission.ViewVehicles);
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
