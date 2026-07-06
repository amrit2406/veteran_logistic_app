using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.ViewModels;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.Navigation;
using Microsoft.Extensions.Logging;

namespace veteran_logistic.Authentication.Services;

/// <summary>
/// Service for handling user logout operations.
/// </summary>
public sealed class LogoutService : ILogoutService
{
    private readonly ISessionManager _sessionManager;
    private readonly IApplicationContext _applicationContext;
    private readonly IFinancialYearContext _financialYearContext;
    private readonly INavigationService _navigationService;
    private readonly ILogger<LogoutService> _logger;

    /// <summary>
    /// Initializes a new instance of the LogoutService.
    /// </summary>
    /// <param name="sessionManager">The session manager.</param>
    /// <param name="applicationContext">The application context.</param>
    /// <param name="financialYearContext">The financial year context.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="logger">The logger.</param>
    public LogoutService(
        ISessionManager sessionManager,
        IApplicationContext applicationContext,
        IFinancialYearContext financialYearContext,
        INavigationService navigationService,
        ILogger<LogoutService> logger)
    {
        _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        _financialYearContext = financialYearContext ?? throw new ArgumentNullException(nameof(financialYearContext));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Logs out the current user by clearing the runtime session and returning to the Login screen.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Logout initiated for user: {Username}", _applicationContext.CurrentUser?.Username ?? "Unknown");

        // Step 1: Clear SessionManager
        _sessionManager.ClearSession();
        _logger.LogDebug("Session cleared");

        // Step 2: Reset AuthenticationState
        _applicationContext.AuthenticationState.IsAuthenticated = false;
        _applicationContext.AuthenticationState.AuthenticatedAt = null;
        _logger.LogDebug("Authentication state reset");

        // Step 3: Clear CurrentUser
        _applicationContext.CurrentUser = null;
        _logger.LogDebug("Current user cleared");

        // Step 4: Clear FinancialYearContext
        _financialYearContext.ClearFinancialYear();
        _logger.LogDebug("Financial year context cleared");

        // Step 5: Navigate to LoginViewModel
        await _navigationService.NavigateAsync<LoginViewModel>();
        _logger.LogInformation("Logout completed and navigated to Login screen");
    }
}
