using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using HsdRateEntity = VeteranLogistics.Data.Entities.Administration.HsdRate;
using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;

namespace veteran_logistic.Masters.HsdRates.Services;

/// <summary>
/// Implementation of the HSD rate query service.
/// </summary>
public sealed class HsdRateQueryService : IHsdRateQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<HsdRateQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsdRateQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public HsdRateQueryService(VeteranLogisticsDbContext dbContext, ILogger<HsdRateQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<HsdRateListItem>> GetAllHsdRatesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.HsdRates.Include(h => h.FuelPump).AsNoTracking();
            var hsdRates = await ProjectToListItem(query)
                .OrderBy(h => h.FuelPumpName)
                .ThenBy(h => h.ApplicableDate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} HSD rates", hsdRates.Count);
            return hsdRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all HSD rates");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<HsdRateListItem>> SearchHsdRatesAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.HsdRates.Include(h => h.FuelPump).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchPattern = $"%{searchTerm}%";
                query = query.Where(h => h.FuelPump != null && EF.Functions.Like(h.FuelPump.FuelPumpName, searchPattern));
            }

            var hsdRates = await ProjectToListItem(query)
                .OrderBy(h => h.FuelPumpName)
                .ThenBy(h => h.ApplicableDate)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} HSD rates matching search term '{SearchTerm}'", hsdRates.Count, searchTerm);
            return hsdRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching HSD rates with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<HsdRateModel?> GetHsdRateForEditAsync(int hsdRateId, CancellationToken cancellationToken = default)
    {
        try
        {
            var hsdRate = await _dbContext.HsdRates
                .AsNoTracking()
                .Where(h => h.Id == hsdRateId)
                .Select(h => new HsdRateModel
                {
                    Id = h.Id,
                    FuelPumpId = h.FuelPumpId,
                    ApplicableDate = h.ApplicableDate,
                    RatePerLitre = h.RatePerLitre,
                    IsActive = h.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (hsdRate is null)
            {
                _logger.LogWarning("HSD rate with ID {HsdRateId} not found", hsdRateId);
                return null;
            }

            _logger.LogInformation("Retrieved HSD rate with ID {HsdRateId} for editing", hsdRateId);
            return hsdRate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving HSD rate with ID {HsdRateId} for editing", hsdRateId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FuelPumpDropdownItem>> GetFuelPumpsForDropdownAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var fuelPumps = await _dbContext.FuelPumps
                .AsNoTracking()
                .Where(f => f.IsActive)
                .OrderBy(f => f.FuelPumpName)
                .Select(f => new FuelPumpDropdownItem
                {
                    Id = f.Id,
                    Name = f.FuelPumpName
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} fuel pumps for dropdown", fuelPumps.Count);
            return fuelPumps;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving fuel pumps for dropdown");
            throw;
        }
    }

    private static IQueryable<HsdRateListItem> ProjectToListItem(IQueryable<HsdRateEntity> query)
    {
        return query.Select(h => new HsdRateListItem
        {
            Id = h.Id,
            FuelPumpId = h.FuelPumpId,
            FuelPumpName = h.FuelPump != null ? h.FuelPump.FuelPumpName : "Unknown",
            ApplicableDate = h.ApplicableDate,
            RatePerLitre = h.RatePerLitre,
            IsActive = h.IsActive
        });
    }
}
