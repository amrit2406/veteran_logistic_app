using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using CompanyEntity = VeteranLogistics.Data.Entities.Administration.Company;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;

namespace veteran_logistic.Masters.Companies.Services;

/// <summary>
/// Implementation of the company query service.
/// </summary>
public sealed class CompanyQueryService : ICompanyQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<CompanyQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public CompanyQueryService(VeteranLogisticsDbContext dbContext, ILogger<CompanyQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CompanyListItem>> GetAllCompaniesAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Companies.AsNoTracking())
            .OrderBy(c => c.CompanyName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CompanyModel?> GetCompanyAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CompanyModel
            {
                Id = c.Id,
                CompanyCode = c.CompanyCode,
                CompanyName = c.CompanyName,
                AddressLine1 = c.AddressLine1,
                AddressLine2 = c.AddressLine2,
                City = c.City,
                State = c.State,
                Country = c.Country,
                PostalCode = c.PostalCode,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                GSTNumber = c.GSTNumber,
                PANNumber = c.PANNumber,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<CompanyListItem> ProjectToListItem(IQueryable<CompanyEntity> query)
    {
        return query.Select(c => new CompanyListItem
        {
            Id = c.Id,
            CompanyCode = c.CompanyCode,
            CompanyName = c.CompanyName,
            GSTNumber = c.GSTNumber,
            PANNumber = c.PANNumber,
            City = c.City,
            State = c.State,
            IsActive = c.IsActive
        });
    }
}
