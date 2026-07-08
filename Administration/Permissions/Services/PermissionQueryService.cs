using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Administration.Permissions.Contracts;
using veteran_logistic.Administration.Permissions.Models;

namespace veteran_logistic.Administration.Permissions.Services;

/// <summary>
/// Implementation of the permission query service.
/// </summary>
public sealed class PermissionQueryService : IPermissionQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PermissionQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PermissionQueryService(VeteranLogisticsDbContext dbContext, ILogger<PermissionQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<PermissionMatrixModel> GetPermissionMatrixAsync(CancellationToken cancellationToken = default)
    {
        // Load roles
        var roles = await _dbContext.Roles
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Load permissions
        var permissions = await _dbContext.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module)
            .ThenBy(p => p.Screen)
            .ThenBy(p => p.SortOrder)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Load role permissions
        var rolePermissions = await _dbContext.RolePermissions
            .AsNoTracking()
            .Where(rp => rp.IsGranted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Build the matrix model
        var model = new PermissionMatrixModel();

        // Add roles with their granted permissions
        foreach (var role in roles)
        {
            var roleItem = new RoleMatrixItem
            {
                RoleId = role.Id,
                RoleName = role.Name,
                GrantedPermissionIds = new HashSet<int>(
                    rolePermissions
                        .Where(rp => rp.RoleId == role.Id)
                        .Select(rp => rp.PermissionId))
            };
            model.Roles.Add(roleItem);
        }

        // Add permissions
        foreach (var permission in permissions)
        {
            model.Permissions.Add(new PermissionMatrixRow
            {
                PermissionId = permission.Id,
                Module = permission.Module,
                Screen = permission.Screen,
                PermissionKey = permission.PermissionKey,
                DisplayName = permission.DisplayName,
                Description = permission.Description,
                SortOrder = permission.SortOrder
            });
        }

        return model;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PermissionListItem>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Permissions
            .AsNoTracking()
            .OrderBy(p => p.Module)
            .ThenBy(p => p.Screen)
            .ThenBy(p => p.SortOrder)
            .Select(p => new PermissionListItem
            {
                Id = p.Id,
                Module = p.Module,
                Screen = p.Screen,
                PermissionKey = p.PermissionKey,
                DisplayName = p.DisplayName,
                Description = p.Description,
                SortOrder = p.SortOrder
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
