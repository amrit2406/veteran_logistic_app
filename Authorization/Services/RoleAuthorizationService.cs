using Microsoft.Extensions.Logging;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Services;

/// <summary>
/// Provides role-based authorization services based on the current authenticated user.
/// </summary>
public sealed class RoleAuthorizationService : IRoleAuthorizationService
{
    private readonly IApplicationContext _applicationContext;
    private readonly ILogger<RoleAuthorizationService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleAuthorizationService"/> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    /// <param name="logger">The logger.</param>
    public RoleAuthorizationService(IApplicationContext applicationContext, ILogger<RoleAuthorizationService> logger)
    {
        _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public bool HasRole(ApplicationRole role)
    {
        var userRole = GetCurrentUserRole();
        return userRole == role;
    }

    /// <inheritdoc />
    public bool HasAnyRole(params ApplicationRole[] roles)
    {
        if (roles == null || roles.Length == 0)
        {
            return false;
        }

        var userRole = GetCurrentUserRole();
        return roles.Contains(userRole);
    }

    /// <inheritdoc />
    public bool HasAllRoles(params ApplicationRole[] roles)
    {
        if (roles == null || roles.Length == 0)
        {
            return false;
        }

        var userRole = GetCurrentUserRole();
        return roles.All(r => r == userRole);
    }

    /// <inheritdoc />
    public bool IsAdministrator()
    {
        return HasRole(ApplicationRole.Administrator);
    }

    private ApplicationRole GetCurrentUserRole()
    {
        var currentUser = _applicationContext.CurrentUser;

        if (currentUser == null)
        {
            _logger.LogDebug("No authenticated user found. Defaulting to None role.");
            return ApplicationRole.None;
        }

        return currentUser.Role;
    }
}
