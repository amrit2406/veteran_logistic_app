using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Administration.Permissions.Contracts;
using veteran_logistic.Administration.Permissions.Models;

namespace veteran_logistic.Administration.Permissions.Services;

/// <summary>
/// Implementation of the permission command service.
/// </summary>
public sealed class PermissionCommandService : IPermissionCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly IAssignPermissionsValidator _validator;
    private readonly ILogger<PermissionCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="validator">The assign permissions validator.</param>
    /// <param name="logger">The logger.</param>
    public PermissionCommandService(
        VeteranLogisticsDbContext dbContext,
        IAssignPermissionsValidator validator,
        ILogger<PermissionCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<AssignPermissionsResult> AssignPermissionsAsync(AssignPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate the request
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return AssignPermissionsResult.Failure(errorMessage);
            }

            // Verify the role exists
            var role = await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken)
                .ConfigureAwait(false);

            if (role is null)
            {
                return AssignPermissionsResult.Failure($"Role with ID {request.RoleId} not found.");
            }

            // Get all permission IDs to validate they exist
            var allPermissionIds = await _dbContext.Permissions
                .AsNoTracking()
                .Select(p => p.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var invalidPermissionIds = request.PermissionAssignments
                .Select(pa => pa.PermissionId)
                .Distinct()
                .Where(pid => !allPermissionIds.Contains(pid))
                .ToList();

            if (invalidPermissionIds.Any())
            {
                return AssignPermissionsResult.Failure($"Invalid permission IDs: {string.Join(", ", invalidPermissionIds)}");
            }

            // Load existing role permissions
            var existingRolePermissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == request.RoleId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            // Create HashSet for efficient lookups
            var existingPermissionsMap = existingRolePermissions
                .ToDictionary(rp => rp.PermissionId, rp => rp);

            var requestedPermissions = request.PermissionAssignments
                .ToDictionary(pa => pa.PermissionId, pa => pa.IsGranted);

            // Calculate differences
            var permissionsToAdd = new List<RolePermission>();
            var permissionsToRemove = new List<RolePermission>();

            foreach (var (permissionId, isGranted) in requestedPermissions)
            {
                if (existingPermissionsMap.TryGetValue(permissionId, out var existingRolePermission))
                {
                    // Permission already exists - check if state changed
                    if (existingRolePermission.IsGranted != isGranted)
                    {
                        if (isGranted)
                        {
                            // Revoked -> Granted: Update existing record
                            existingRolePermission.IsGranted = true;
                        }
                        else
                        {
                            // Granted -> Revoked: Remove the record
                            permissionsToRemove.Add(existingRolePermission);
                        }
                    }
                    // If state unchanged, do nothing
                }
                else
                {
                    // New permission assignment
                    if (isGranted)
                    {
                        // Only add if granted
                        permissionsToAdd.Add(new RolePermission
                        {
                            RoleId = request.RoleId,
                            PermissionId = permissionId,
                            IsGranted = true,
                            CreatedOn = DateTime.UtcNow
                        });
                    }
                    // If not granted, don't create a record
                }
            }

            // Apply changes
            if (permissionsToAdd.Any())
            {
                await _dbContext.RolePermissions.AddRangeAsync(permissionsToAdd, cancellationToken).ConfigureAwait(false);
            }

            if (permissionsToRemove.Any())
            {
                _dbContext.RolePermissions.RemoveRange(permissionsToRemove);
            }

            // Single SaveChangesAsync
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return AssignPermissionsResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while assigning permissions to role '{RoleId}'", request.RoleId);
            return AssignPermissionsResult.Failure("An unexpected error occurred while assigning permissions.");
        }
    }
}
