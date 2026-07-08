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
    private readonly ILogger<RoleCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The role creation validator.</param>
    /// <param name="logger">The logger.</param>
    public RoleCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateRoleValidator createValidator,
        ILogger<RoleCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
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
}
