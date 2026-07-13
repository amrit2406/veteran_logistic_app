using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PaymentLocationEntity = VeteranLogistics.Data.Entities.Administration.PaymentLocation;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;

namespace veteran_logistic.Masters.PaymentLocations.Services;

/// <summary>
/// Implementation of the payment location query service.
/// </summary>
public sealed class PaymentLocationQueryService : IPaymentLocationQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PaymentLocationQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentLocationQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PaymentLocationQueryService(VeteranLogisticsDbContext dbContext, ILogger<PaymentLocationQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaymentLocationListItem>> GetAllPaymentLocationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentLocations = await ProjectToListItem(_dbContext.PaymentLocations.AsNoTracking())
                .OrderBy(p => p.PaymentLocationName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} payment locations", paymentLocations.Count);
            return paymentLocations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all payment locations");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaymentLocationListItem>> SearchPaymentLocationsAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.PaymentLocations.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchPattern = $"%{searchTerm}%";
                query = query.Where(p => EF.Functions.Like(p.PaymentLocationName, searchPattern));
            }

            var paymentLocations = await ProjectToListItem(query)
                .OrderBy(p => p.PaymentLocationName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} payment locations matching search term '{SearchTerm}'", paymentLocations.Count, searchTerm);
            return paymentLocations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching payment locations with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PaymentLocationModel?> GetPaymentLocationForEditAsync(int paymentLocationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentLocation = await _dbContext.PaymentLocations
                .AsNoTracking()
                .Where(p => p.Id == paymentLocationId)
                .Select(p => new PaymentLocationModel
                {
                    Id = p.Id,
                    PaymentLocationName = p.PaymentLocationName,
                    IsActive = p.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (paymentLocation is null)
            {
                _logger.LogWarning("Payment location with ID {PaymentLocationId} not found", paymentLocationId);
                return null;
            }

            _logger.LogInformation("Retrieved payment location with ID {PaymentLocationId} for editing", paymentLocationId);
            return paymentLocation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving payment location with ID {PaymentLocationId} for editing", paymentLocationId);
            throw;
        }
    }

    private static IQueryable<PaymentLocationListItem> ProjectToListItem(IQueryable<PaymentLocationEntity> query)
    {
        return query.Select(p => new PaymentLocationListItem
        {
            Id = p.Id,
            PaymentLocationName = p.PaymentLocationName,
            IsActive = p.IsActive
        });
    }
}
