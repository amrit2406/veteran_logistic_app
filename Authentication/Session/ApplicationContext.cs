using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Session;

/// <summary>
/// Stores the current application context only.
/// </summary>
public sealed class ApplicationContext : ObservableObject, IApplicationContext
{
    private AuthenticatedUser? _currentUser;
    private AuthenticationState _authenticationState;

    private readonly IFinancialYearContext _financialYearContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
    /// </summary>
    /// <param name="authenticationState">The shared authentication state instance.</param>
    /// <param name="financialYearContext">The financial year context.</param>
    public ApplicationContext(AuthenticationState authenticationState, IFinancialYearContext financialYearContext)
    {
        _authenticationState = authenticationState ?? throw new ArgumentNullException(nameof(authenticationState));
        _financialYearContext = financialYearContext ?? throw new ArgumentNullException(nameof(financialYearContext));
    }

    /// <summary>
    /// Gets or sets the current authenticated user.
    /// </summary>
    public AuthenticatedUser? CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    /// <summary>
    /// Gets or sets the runtime authentication state.
    /// </summary>
    public AuthenticationState AuthenticationState
    {
        get => _authenticationState;
        set => SetProperty(ref _authenticationState, value ?? throw new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Gets the financial year context.
    /// </summary>
    public IFinancialYearContext FinancialYearContext => _financialYearContext;
}
