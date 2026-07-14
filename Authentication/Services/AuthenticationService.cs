using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using veteran_logistic.Authentication.Validation;
using veteran_logistic.Authorization.Models;
using VeteranLogistics.Data.Entities.Administration;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Authentication.Services;

/// <summary>
/// Service for handling user authentication.
/// </summary>
public sealed class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationState _authenticationState;
    private readonly ISessionManager _sessionManager;
    private readonly IApplicationContext _applicationContext;
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthenticationAuditService _authenticationAuditService;

    /// <summary>
    /// Initializes a new instance of the AuthenticationService.
    /// </summary>
    /// <param name="authenticationState">The authentication state.</param>
    /// <param name="sessionManager">The session manager.</param>
    /// <param name="applicationContext">The application context.</param>
    /// <param name="authenticationRepository">The authentication repository.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="authenticationAuditService">The authentication audit service.</param>
    public AuthenticationService(
        AuthenticationState authenticationState,
        ISessionManager sessionManager,
        IApplicationContext applicationContext,
        IAuthenticationRepository authenticationRepository,
        IPasswordHasher passwordHasher,
        IAuthenticationAuditService authenticationAuditService)
    {
        _authenticationState = authenticationState ?? throw new ArgumentNullException(nameof(authenticationState));
        _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        _authenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _authenticationAuditService = authenticationAuditService ?? throw new ArgumentNullException(nameof(authenticationAuditService));
    }

    /// <summary>
    /// Gets the current authentication state for the running application.
    /// </summary>
    public AuthenticationState AuthenticationState => _authenticationState;

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating whether authentication was successful.</returns>
    public async Task<AuthenticationResult> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        // Step 1: Validate the login request
        var validationResult = LoginRequestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            var errorMessage = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            await LogAuditEntryAsync(request.Username, false, errorMessage, cancellationToken);
            return AuthenticationResult.Failure(errorMessage);
        }

        // Step 2: Retrieve the user from the repository
        var user = await _authenticationRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user is null)
        {
            await LogAuditEntryAsync(request.Username, false, "Invalid username or password.", cancellationToken);
            return AuthenticationResult.Failure("Invalid username or password.");
        }

        // Step 3: Verify the password
        var passwordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
        if (!passwordValid)
        {
            await LogAuditEntryAsync(request.Username, false, "Invalid username or password.", cancellationToken);
            return AuthenticationResult.Failure("Invalid username or password.");
        }

        // Step 4: Check if user is active
        if (!user.IsActive)
        {
            await LogAuditEntryAsync(request.Username, false, "User account is inactive.", cancellationToken);
            return AuthenticationResult.Failure("User account is inactive.");
        }

        // Step 5: Record successful authentication attempt
        await LogAuditEntryAsync(request.Username, true, null, cancellationToken);

        // Step 6: Create authenticated user
        var authenticatedUser = new AuthenticatedUser
        {
            UserId = user.Id.ToString(),
            Username = user.Username,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Role = user.Role ?? string.Empty
        };

        // Step 7: Create user session
        var session = new UserSession
        {
            SessionId = Guid.NewGuid().ToString(),
            StartedAt = DateTimeOffset.UtcNow,
            LastActivityAt = DateTimeOffset.UtcNow,
            IsActive = true,
            LoginTime = DateTimeOffset.UtcNow,
            LastActivity = DateTimeOffset.UtcNow,
            AuthenticatedUser = authenticatedUser
        };

        // Step 8: Store session in session manager
        _sessionManager.CurrentSession = session;

        // Step 9: Update application context
        _applicationContext.CurrentUser = authenticatedUser;
        _applicationContext.AuthenticationState = _authenticationState;

        // Step 10: Set authentication state to authenticated
        _authenticationState.IsAuthenticated = true;
        _authenticationState.AuthenticatedAt = DateTimeOffset.UtcNow;

        return AuthenticationResult.Success();
    }

    private async Task LogAuditEntryAsync(string username, bool success, string? failureReason, CancellationToken cancellationToken)
    {
        var auditEntry = new AuthenticationAuditEntry
        {
            Username = username,
            Timestamp = DateTimeOffset.UtcNow,
            Success = success,
            FailureReason = failureReason
        };

        await _authenticationAuditService.LogAuthenticationAttemptAsync(auditEntry, cancellationToken);
    }
}
