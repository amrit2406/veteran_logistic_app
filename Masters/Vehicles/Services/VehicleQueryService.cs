using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleEntity = VeteranLogistics.Data.Entities.Administration.Vehicle;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;

namespace veteran_logistic.Masters.Vehicles.Services;

/// <summary>
/// Implementation of the vehicle query service.
/// </summary>
public sealed class VehicleQueryService : IVehicleQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<VehicleQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public VehicleQueryService(VeteranLogisticsDbContext dbContext, ILogger<VehicleQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleListItem>> GetAllVehiclesAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Vehicles.AsNoTracking()
            .Include(v => v.VehicleOwner))
            .OrderBy(v => v.VehicleNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleListItem>> SearchVehiclesAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Vehicles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(v =>
                EF.Functions.Like(v.VehicleNumber, searchPattern) ||
                EF.Functions.Like(v.VehicleType, searchPattern));
        }

        var vehicles = await query
            .Include(v => v.VehicleOwner)
            .OrderBy(v => v.VehicleNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return vehicles.Select(v => new VehicleListItem
        {
            Id = v.Id,
            VehicleNumber = v.VehicleNumber,
            VehicleType = v.VehicleType,
            OwnerCode = v.VehicleOwner != null ? v.VehicleOwner.OwnerCode : string.Empty,
            OwnerName = v.VehicleOwner != null 
                ? (!string.IsNullOrWhiteSpace(v.VehicleOwner.CompanyName) 
                    ? v.VehicleOwner.CompanyName 
                    : $"{v.VehicleOwner.FirstName} {v.VehicleOwner.LastName}".Trim())
                : string.Empty,
            IsActive = v.IsActive
        }).ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<VehicleModel?> GetVehicleForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vehicles
            .AsNoTracking()
            .Where(v => v.Id == id)
            .Select(v => new VehicleModel
            {
                Id = v.Id,
                VehicleOwnerId = v.VehicleOwnerId,
                VehicleNumber = v.VehicleNumber,
                VehicleType = v.VehicleType,
                IsActive = v.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<VehicleListItem> ProjectToListItem(IQueryable<VehicleEntity> query)
    {
        return query.Select(v => new VehicleListItem
        {
            Id = v.Id,
            VehicleNumber = v.VehicleNumber,
            VehicleType = v.VehicleType,
            OwnerCode = v.VehicleOwner != null ? v.VehicleOwner.OwnerCode : string.Empty,
            OwnerName = v.VehicleOwner != null 
                ? (!string.IsNullOrWhiteSpace(v.VehicleOwner.CompanyName) 
                    ? v.VehicleOwner.CompanyName 
                    : $"{v.VehicleOwner.FirstName} {v.VehicleOwner.LastName}".Trim())
                : string.Empty,
            IsActive = v.IsActive
        });
    }
}
