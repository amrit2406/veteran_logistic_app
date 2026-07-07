using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;

namespace veteran_logistic.Administration.Roles.Services;

/// <summary>
/// Implementation of the role query service.
/// </summary>
public sealed class RoleQueryService : IRoleQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<RoleQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public RoleQueryService(VeteranLogisticsDbContext dbContext, ILogger<RoleQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<RoleListItem>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Roles.AsNoTracking())
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<RoleListItem> ProjectToListItem(IQueryable<Role> query)
    {
        return query.Select(r => new RoleListItem
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsActive = r.IsActive,
            IsSystemRole = r.IsSystemRole,
            CreatedOn = r.CreatedOn
        });
    }
}
