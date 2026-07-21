using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Services;

/// <summary>
/// Service for querying DO Rate data.
/// </summary>
public sealed class DORateQueryService : IDORateQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<DORateQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DORateQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public DORateQueryService(VeteranLogisticsDbContext dbContext, ILogger<DORateQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DORateListItem>> GetAllDORatesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all DO rates");

        var doRates = await _dbContext.DORates
            .Include(d => d.Source)
            .Include(d => d.Destination)
            .AsNoTracking()
            .Select(d => new DORateListItem
            {
                Id = d.Id,
                Consignor = _dbContext.Customers.Where(c => c.Id == d.ConsignorId).Select(c => c.CustomerName).FirstOrDefault() ?? string.Empty,
                Consignee = _dbContext.Customers.Where(c => c.Id == d.ConsigneeId).Select(c => c.CustomerName).FirstOrDefault() ?? string.Empty,
                Source = d.Source != null ? d.Source.LocationName : string.Empty,
                Destination = d.Destination != null ? d.Destination.LocationName : string.Empty,
                EffectiveDate = d.EffectiveDate,
                FreightRate = d.FreightRate,
                VendorRate = d.VendorRate,
                BillingRate = d.BillingRate,
                DONumber = d.DONumber,
                IsActive = d.IsActive
            })
            .OrderBy(d => d.DONumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Retrieved {Count} DO rates", doRates.Count);

        return doRates;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DORateListItem>> SearchDORatesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching DO rates with term: {SearchTerm}", searchTerm);

        var query = _dbContext.DORates
            .Include(d => d.Source)
            .Include(d => d.Destination)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchPattern = $"%{searchTerm}%";
            query = query.Where(d =>
                EF.Functions.Like(d.DONumber, searchPattern) ||
                (d.Source != null && EF.Functions.Like(d.Source.LocationName, searchPattern)) ||
                (d.Destination != null && EF.Functions.Like(d.Destination.LocationName, searchPattern)) ||
                EF.Functions.Like(d.VesselName, searchPattern) ||
                EF.Functions.Like(d.TraderName, searchPattern));
        }

        var doRates = await query
            .Select(d => new DORateListItem
            {
                Id = d.Id,
                Consignor = _dbContext.Customers.Where(c => c.Id == d.ConsignorId).Select(c => c.CustomerName).FirstOrDefault() ?? string.Empty,
                Consignee = _dbContext.Customers.Where(c => c.Id == d.ConsigneeId).Select(c => c.CustomerName).FirstOrDefault() ?? string.Empty,
                Source = d.Source != null ? d.Source.LocationName : string.Empty,
                Destination = d.Destination != null ? d.Destination.LocationName : string.Empty,
                EffectiveDate = d.EffectiveDate,
                FreightRate = d.FreightRate,
                VendorRate = d.VendorRate,
                BillingRate = d.BillingRate,
                DONumber = d.DONumber,
                IsActive = d.IsActive
            })
            .OrderBy(d => d.DONumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Search returned {Count} DO rates", doRates.Count);

        return doRates;
    }

    /// <inheritdoc />
    public async Task<DORateModel?> GetDORateForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting DO rate for edit with ID: {Id}", id);

        var doRate = await _dbContext.DORates
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DORateModel
            {
                Id = d.Id,
                ConsignorId = d.ConsignorId,
                ConsigneeId = d.ConsigneeId,
                SourceId = d.SourceId,
                DestinationId = d.DestinationId,
                EffectiveDate = d.EffectiveDate,
                FreightRate = d.FreightRate,
                UnionRate = d.UnionRate,
                VendorRate = d.VendorRate,
                DONumber = d.DONumber,
                BillingRate = d.BillingRate,
                AllowedShortage = d.AllowedShortage,
                RatePerKg = d.RatePerKg,
                VesselName = d.VesselName,
                TraderName = d.TraderName,
                Narration = d.Narration,
                IsActive = d.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (doRate == null)
        {
            _logger.LogWarning("DO Rate with ID {Id} not found", id);
        }

        return doRate;
    }
}
