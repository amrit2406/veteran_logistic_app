using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;

namespace veteran_logistic.Masters.SourceDestinations.Services;

/// <summary>
/// Service for querying source/destination data.
/// </summary>
public sealed class SourceDestinationQueryService : ISourceDestinationQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<SourceDestinationQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceDestinationQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public SourceDestinationQueryService(VeteranLogisticsDbContext dbContext, ILogger<SourceDestinationQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SourceDestinationListItem>> GetAllSourceDestinationsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all source/destinations");

        var sourceDestinations = await _dbContext.SourceDestinations
            .Select(sd => new SourceDestinationListItem
            {
                Id = sd.Id,
                LocationName = sd.LocationName,
                IsActive = sd.IsActive
            })
            .OrderBy(sd => sd.LocationName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Retrieved {Count} source/destinations", sourceDestinations.Count);

        return sourceDestinations;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SourceDestinationListItem>> SearchSourceDestinationsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching source/destinations with term: {SearchTerm}", searchTerm);

        var query = _dbContext.SourceDestinations.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(sd => sd.LocationName.Contains(searchTerm));
        }

        var sourceDestinations = await query
            .Select(sd => new SourceDestinationListItem
            {
                Id = sd.Id,
                LocationName = sd.LocationName,
                IsActive = sd.IsActive
            })
            .OrderBy(sd => sd.LocationName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Search returned {Count} source/destinations", sourceDestinations.Count);

        return sourceDestinations;
    }

    /// <inheritdoc />
    public async Task<SourceDestinationModel?> GetSourceDestinationForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting source/destination for edit with ID: {Id}", id);

        var sourceDestination = await _dbContext.SourceDestinations
            .Where(sd => sd.Id == id)
            .Select(sd => new SourceDestinationModel
            {
                Id = sd.Id,
                LocationName = sd.LocationName,
                IsActive = sd.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (sourceDestination == null)
        {
            _logger.LogWarning("Source/Destination with ID {Id} not found", id);
        }

        return sourceDestination;
    }
}
