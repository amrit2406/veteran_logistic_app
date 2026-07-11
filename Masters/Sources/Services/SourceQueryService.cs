using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using SourceEntity = VeteranLogistics.Data.Entities.Administration.Source;
using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;

namespace veteran_logistic.Masters.Sources.Services;

/// <summary>
/// Implementation of the source query service.
/// </summary>
public sealed class SourceQueryService : ISourceQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<SourceQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public SourceQueryService(VeteranLogisticsDbContext dbContext, ILogger<SourceQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SourceListItem>> GetAllSourcesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var sources = await _dbContext.Sources
                .AsNoTracking()
                .OrderBy(s => s.SourceName)
                .Select(s => new SourceListItem
                {
                    Id = s.Id,
                    SourceCode = s.SourceCode,
                    SourceName = s.SourceName,
                    City = s.City,
                    State = s.State,
                    ContactPerson = s.ContactPerson,
                    PhoneNumber = s.PhoneNumber,
                    IsActive = s.IsActive
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} sources", sources.Count);
            return sources;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all sources");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SourceListItem>> SearchSourcesAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var sources = await _dbContext.Sources
                .AsNoTracking()
                .Where(s => EF.Functions.Like(s.SourceCode, $"%{searchTerm}%")
                         || EF.Functions.Like(s.SourceName, $"%{searchTerm}%")
                         || EF.Functions.Like(s.City, $"%{searchTerm}%")
                         || EF.Functions.Like(s.State, $"%{searchTerm}%"))
                .OrderBy(s => s.SourceName)
                .Select(s => new SourceListItem
                {
                    Id = s.Id,
                    SourceCode = s.SourceCode,
                    SourceName = s.SourceName,
                    City = s.City,
                    State = s.State,
                    ContactPerson = s.ContactPerson,
                    PhoneNumber = s.PhoneNumber,
                    IsActive = s.IsActive
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} sources matching search term '{SearchTerm}'", sources.Count, searchTerm);
            return sources;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching sources with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<SourceModel?> GetSourceForEditAsync(int sourceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var source = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == sourceId, cancellationToken)
                .ConfigureAwait(false);

            if (source is null)
            {
                _logger.LogWarning("Source with ID {SourceId} not found", sourceId);
                return null;
            }

            var sourceModel = new SourceModel
            {
                Id = source.Id,
                SourceCode = source.SourceCode,
                SourceName = source.SourceName,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                State = source.State,
                Country = source.Country,
                PostalCode = source.PostalCode,
                ContactPerson = source.ContactPerson,
                PhoneNumber = source.PhoneNumber,
                Email = source.Email,
                GSTNumber = source.GSTNumber,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                Remarks = source.Remarks,
                IsActive = source.IsActive
            };

            _logger.LogInformation("Retrieved source with ID {SourceId} for editing", sourceId);
            return sourceModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving source with ID {SourceId} for editing", sourceId);
            throw;
        }
    }
}
