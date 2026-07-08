using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;

namespace veteran_logistic.Administration.Roles.Services;

/// <summary>
/// Implementation of the role command service.
/// </summary>
public sealed class RoleCommandService : IRoleCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateRoleValidator _createValidator;
    private readonly IUpdateRoleValidator _updateValidator;
    private readonly ILogger<RoleCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The role creation validator.</param>
    /// <param name="updateValidator">The role update validator.</param>
    /// <param name="logger">The logger.</param>
    public RoleCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateRoleValidator createValidator,
        IUpdateRoleValidator updateValidator,
        ILogger<RoleCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateRoleResult> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateRoleResult.Failure(errorMessage);
            }

            var trimmedName = request.Name.Trim();

            var existingRole = await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == trimmedName, cancellationToken)
                .ConfigureAwait(false);

            if (existingRole is not null)
            {
                return CreateRoleResult.Failure("A role with this name already exists.");
            }

            var role = new Role
            {
                Name = trimmedName,
                Description = request.Description?.Trim() ?? string.Empty,
                IsActive = request.IsActive,
                IsSystemRole = false,
                CreatedOn = DateTime.UtcNow
            };

            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CreateRoleResult.Success(role.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating role '{Name}'", request.Name);
            return CreateRoleResult.Failure("An unexpected error occurred while creating the role.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateRoleResult> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateRoleResult.Failure(errorMessage);
            }

            var role = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (role is null)
            {
                return UpdateRoleResult.Failure("Role not found.");
            }

            var trimmedName = request.Name.Trim();

            var existingRole = await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == trimmedName && r.Id != request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (existingRole is not null)
            {
                return UpdateRoleResult.Failure("A role with this name already exists.");
            }

            role.Name = trimmedName;
            role.Description = request.Description?.Trim() ?? string.Empty;
            role.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return UpdateRoleResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating role '{Id}'", request.Id);
            return UpdateRoleResult.Failure("An unexpected error occurred while updating the role.");
        }
    }
}
