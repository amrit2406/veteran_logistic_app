using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using FuelPumpEntity = VeteranLogistics.Data.Entities.Administration.FuelPump;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;

namespace veteran_logistic.Masters.FuelPumps.Services;

/// <summary>
/// Implementation of the fuel pump query service.
/// </summary>
public sealed class FuelPumpQueryService : IFuelPumpQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<FuelPumpQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuelPumpQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public FuelPumpQueryService(VeteranLogisticsDbContext dbContext, ILogger<FuelPumpQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FuelPumpListItem>> GetAllFuelPumpsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var fuelPumps = await ProjectToListItem(_dbContext.FuelPumps.AsNoTracking())
                .OrderBy(f => f.FuelPumpName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} fuel pumps", fuelPumps.Count);
            return fuelPumps;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all fuel pumps");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FuelPumpListItem>> SearchFuelPumpsAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.FuelPumps.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchPattern = $"%{searchTerm}%";
                query = query.Where(f => EF.Functions.Like(f.FuelPumpName, searchPattern));
            }

            var fuelPumps = await ProjectToListItem(query)
                .OrderBy(f => f.FuelPumpName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} fuel pumps matching search term '{SearchTerm}'", fuelPumps.Count, searchTerm);
            return fuelPumps;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching fuel pumps with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<FuelPumpModel?> GetFuelPumpForEditAsync(int fuelPumpId, CancellationToken cancellationToken = default)
    {
        try
        {
            var fuelPump = await _dbContext.FuelPumps
                .AsNoTracking()
                .Where(f => f.Id == fuelPumpId)
                .Select(f => new FuelPumpModel
                {
                    Id = f.Id,
                    FuelPumpName = f.FuelPumpName,
                    IsActive = f.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                _logger.LogWarning("Fuel pump with ID {FuelPumpId} not found", fuelPumpId);
                return null;
            }

            _logger.LogInformation("Retrieved fuel pump with ID {FuelPumpId} for editing", fuelPumpId);
            return fuelPump;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving fuel pump with ID {FuelPumpId} for editing", fuelPumpId);
            throw;
        }
    }

    private static IQueryable<FuelPumpListItem> ProjectToListItem(IQueryable<FuelPumpEntity> query)
    {
        return query.Select(f => new FuelPumpListItem
        {
            Id = f.Id,
            FuelPumpName = f.FuelPumpName,
            IsActive = f.IsActive
        });
    }
}
