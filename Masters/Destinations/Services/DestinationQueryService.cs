using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using DestinationEntity = VeteranLogistics.Data.Entities.Administration.Destination;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;

namespace veteran_logistic.Masters.Destinations.Services;

/// <summary>
/// Implementation of the destination query service.
/// </summary>
public sealed class DestinationQueryService : IDestinationQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<DestinationQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public DestinationQueryService(VeteranLogisticsDbContext dbContext, ILogger<DestinationQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DestinationListItem>> GetAllDestinationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var destinations = await _dbContext.Destinations
                .AsNoTracking()
                .OrderBy(d => d.DestinationName)
                .Select(d => new DestinationListItem
                {
                    Id = d.Id,
                    DestinationCode = d.DestinationCode,
                    DestinationName = d.DestinationName,
                    City = d.City,
                    State = d.State,
                    ContactPerson = d.ContactPerson,
                    PhoneNumber = d.PhoneNumber,
                    IsActive = d.IsActive
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} destinations", destinations.Count);
            return destinations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all destinations");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<DestinationListItem>> SearchDestinationsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var destinations = await _dbContext.Destinations
                .AsNoTracking()
                .Where(d => EF.Functions.Like(d.DestinationCode, $"%{searchTerm}%")
                         || EF.Functions.Like(d.DestinationName, $"%{searchTerm}%")
                         || EF.Functions.Like(d.City, $"%{searchTerm}%")
                         || EF.Functions.Like(d.State, $"%{searchTerm}%"))
                .OrderBy(d => d.DestinationName)
                .Select(d => new DestinationListItem
                {
                    Id = d.Id,
                    DestinationCode = d.DestinationCode,
                    DestinationName = d.DestinationName,
                    City = d.City,
                    State = d.State,
                    ContactPerson = d.ContactPerson,
                    PhoneNumber = d.PhoneNumber,
                    IsActive = d.IsActive
                })
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} destinations matching search term '{SearchTerm}'", destinations.Count, searchTerm);
            return destinations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching destinations with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<DestinationModel?> GetDestinationForEditAsync(int destinationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var destination = await _dbContext.Destinations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == destinationId, cancellationToken)
                .ConfigureAwait(false);

            if (destination is null)
            {
                _logger.LogWarning("Destination with ID {DestinationId} not found", destinationId);
                return null;
            }

            var destinationModel = new DestinationModel
            {
                Id = destination.Id,
                DestinationCode = destination.DestinationCode,
                DestinationName = destination.DestinationName,
                AddressLine1 = destination.AddressLine1,
                AddressLine2 = destination.AddressLine2,
                City = destination.City,
                State = destination.State,
                Country = destination.Country,
                PostalCode = destination.PostalCode,
                ContactPerson = destination.ContactPerson,
                PhoneNumber = destination.PhoneNumber,
                Email = destination.Email,
                GSTNumber = destination.GSTNumber,
                Latitude = destination.Latitude,
                Longitude = destination.Longitude,
                Remarks = destination.Remarks,
                IsActive = destination.IsActive
            };

            _logger.LogInformation("Retrieved destination with ID {DestinationId} for editing", destinationId);
            return destinationModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving destination with ID {DestinationId} for editing", destinationId);
            throw;
        }
    }
}
