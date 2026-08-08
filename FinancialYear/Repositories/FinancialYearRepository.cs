using Microsoft.EntityFrameworkCore;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Repositories;

/// <summary>
/// Repository implementation for retrieving financial years from the database with fallback to default data.
/// </summary>
public sealed class FinancialYearRepository : IFinancialYearRepository
{
    private readonly VeteranLogisticsDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public FinancialYearRepository(VeteranLogisticsDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>> GetActiveFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        // Get financial years from database
        var dbFinancialYears = await _dbContext.FinancialYears
            .Where(fy => !fy.IsDeleted && !fy.IsClosed)
            .OrderByDescending(fy => fy.StartDate)
            .ToListAsync(cancellationToken);

        // Convert database financial years to model
        var result = dbFinancialYears.Select(fy => new veteran_logistic.FinancialYear.Models.FinancialYear
        {
            Id = fy.Id,
            Name = fy.Name,
            StartDate = fy.StartDate,
            EndDate = fy.EndDate,
            IsActive = !fy.IsClosed && !fy.IsDeleted
        }).ToList();

        // Always add 1 default financial year (dummy data)
        var currentYear = DateTime.Now.Year;
        var nextYear = DateTime.Now.Year + 1;
        var defaultYear = new veteran_logistic.FinancialYear.Models.FinancialYear
        {
            Id = -1, // Negative ID indicates this is a default/fallback record
            Name = $"{currentYear}-{nextYear}",
            StartDate = new DateTime(currentYear, 4, 1),
            EndDate = new DateTime(nextYear, 3, 31),
            IsActive = true
        };

        result.Add(defaultYear);

        // Return combined list (database years + default year)
        return result.OrderByDescending(fy => fy.StartDate);
    }
}
