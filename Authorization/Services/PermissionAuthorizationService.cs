using Microsoft.Extensions.Logging;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Services;

/// <summary>
/// Provides permission-based authorization services based on the current authenticated user's role.
/// </summary>
public sealed class PermissionAuthorizationService : IPermissionAuthorizationService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IPermissionProvider _permissionProvider;
    private readonly ILogger<PermissionAuthorizationService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionAuthorizationService"/> class.
    /// </summary>
    /// <param name="applicationContext">The application context.</param>
    /// <param name="permissionProvider">The permission provider.</param>
    /// <param name="logger">The logger.</param>
    public PermissionAuthorizationService(
        IApplicationContext applicationContext,
        IPermissionProvider permissionProvider,
        ILogger<PermissionAuthorizationService> logger)
    {
        _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        _permissionProvider = permissionProvider ?? throw new ArgumentNullException(nameof(permissionProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public bool HasPermission(ApplicationPermission permission)
    {
        var userPermissions = GetUserPermissions();
        return userPermissions.Contains(permission);
    }

    /// <inheritdoc />
    public bool HasAnyPermission(params ApplicationPermission[] permissions)
    {
        if (permissions == null || permissions.Length == 0)
        {
            return false;
        }

        var userPermissions = GetUserPermissions();
        return permissions.Any(p => userPermissions.Contains(p));
    }

    /// <inheritdoc />
    public bool HasAllPermissions(params ApplicationPermission[] permissions)
    {
        if (permissions == null || permissions.Length == 0)
        {
            return false;
        }

        var userPermissions = GetUserPermissions();
        return permissions.All(p => userPermissions.Contains(p));
    }

    private IEnumerable<ApplicationPermission> GetUserPermissions()
    {
        var currentUser = _applicationContext.CurrentUser;

        if (currentUser == null)
        {
            _logger.LogDebug("No authenticated user found. Returning no permissions.");
            return Enumerable.Empty<ApplicationPermission>();
        }

        var permissions = _permissionProvider.GetPermissions(currentUser.Role);
        _logger.LogDebug("User '{Username}' with role '{Role}' has {PermissionCount} permissions.", 
            currentUser.Username, currentUser.Role, permissions.Count());
        
        return permissions;
    }
}
