using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using FinancialYearEntity = VeteranLogistics.Data.Entities.Administration.FinancialYear;
using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;

namespace veteran_logistic.Administration.FinancialYears.Services;

/// <summary>
/// Implementation of the financial year query service.
/// </summary>
public sealed class FinancialYearQueryService : IFinancialYearQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<FinancialYearQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public FinancialYearQueryService(VeteranLogisticsDbContext dbContext, ILogger<FinancialYearQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FinancialYearListItem>> GetAllFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.FinancialYears.AsNoTracking())
            .OrderBy(fy => fy.StartDate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<FinancialYearModel?> GetFinancialYearAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FinancialYears
            .AsNoTracking()
            .Where(fy => fy.Id == id)
            .Select(fy => new FinancialYearModel
            {
                Id = fy.Id,
                Name = fy.Name,
                StartDate = fy.StartDate,
                EndDate = fy.EndDate,
                IsCurrent = fy.IsCurrent,
                IsClosed = fy.IsClosed
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<FinancialYearListItem> ProjectToListItem(IQueryable<FinancialYearEntity> query)
    {
        return query.Select(fy => new FinancialYearListItem
        {
            Id = fy.Id,
            Name = fy.Name,
            StartDate = fy.StartDate,
            EndDate = fy.EndDate,
            IsCurrent = fy.IsCurrent,
            IsClosed = fy.IsClosed
        });
    }
}
