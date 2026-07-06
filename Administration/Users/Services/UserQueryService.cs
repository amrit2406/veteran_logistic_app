using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;

namespace veteran_logistic.Administration.Users.Services;

/// <summary>
/// Implementation of the user query service.
/// </summary>
public sealed class UserQueryService : IUserQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<UserQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public UserQueryService(VeteranLogisticsDbContext dbContext, ILogger<UserQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserListItem>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Users.AsNoTracking())
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UserListItem>> SearchUsersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(u =>
                EF.Functions.Like(u.Username, searchPattern) ||
                (u.DisplayName != null && EF.Functions.Like(u.DisplayName, searchPattern)) ||
                (u.Role != null && EF.Functions.Like(u.Role, searchPattern)));
        }

        return await ProjectToListItem(query)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<UserListItem> ProjectToListItem(IQueryable<User> query)
    {
        return query.Select(u => new UserListItem
        {
            Id = u.Id,
            Username = u.Username,
            DisplayName = u.DisplayName,
            RoleName = u.Role,
            IsActive = u.IsActive,
            CreatedOn = u.CreatedOn
        });
    }
}
