using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
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
    private readonly IDummyLookupService _dummyLookupService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DORateQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="dummyLookupService">The dummy lookup service.</param>
    public DORateQueryService(VeteranLogisticsDbContext dbContext, ILogger<DORateQueryService> logger, IDummyLookupService dummyLookupService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dummyLookupService = dummyLookupService ?? throw new ArgumentNullException(nameof(dummyLookupService));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DORateListItem>> GetAllDORatesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all DO rates");

        var doRates = await _dbContext.DORates
            .Include(d => d.Source)
            .Include(d => d.Destination)
            .Select(d => new DORateListItem
            {
                Id = d.Id,
                Consignor = _dummyLookupService.GetConsignorName(d.ConsignorId),
                Consignee = _dummyLookupService.GetConsigneeName(d.ConsigneeId),
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
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(d =>
                d.DONumber.Contains(searchTerm) ||
                (d.Source != null && d.Source.LocationName.Contains(searchTerm)) ||
                (d.Destination != null && d.Destination.LocationName.Contains(searchTerm)) ||
                d.VesselName.Contains(searchTerm) ||
                d.TraderName.Contains(searchTerm));
        }

        var doRates = await query
            .Select(d => new DORateListItem
            {
                Id = d.Id,
                Consignor = _dummyLookupService.GetConsignorName(d.ConsignorId),
                Consignee = _dummyLookupService.GetConsigneeName(d.ConsigneeId),
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
