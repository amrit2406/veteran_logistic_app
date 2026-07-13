using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleOwnerEntity = VeteranLogistics.Data.Entities.Administration.VehicleOwner;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;

namespace veteran_logistic.Masters.VehicleOwners.Services;

/// <summary>
/// Implementation of the vehicle owner query service.
/// </summary>
public sealed class VehicleOwnerQueryService : IVehicleOwnerQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<VehicleOwnerQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleOwnerQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public VehicleOwnerQueryService(VeteranLogisticsDbContext dbContext, ILogger<VehicleOwnerQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleOwnerListItem>> GetAllVehicleOwnersAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.VehicleOwners.AsNoTracking())
            .OrderBy(o => o.OwnerCode)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleOwnerListItem>> SearchVehicleOwnersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.VehicleOwners.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(o =>
                EF.Functions.Like(o.OwnerCode, searchPattern) ||
                EF.Functions.Like(o.FirstName, searchPattern) ||
                EF.Functions.Like(o.LastName, searchPattern) ||
                EF.Functions.Like(o.CompanyName, searchPattern) ||
                EF.Functions.Like(o.PANNumber, searchPattern) ||
                EF.Functions.Like(o.Mobile, searchPattern) ||
                EF.Functions.Like(o.City, searchPattern));
        }

        return await ProjectToListItem(query)
            .OrderBy(o => o.OwnerCode)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<VehicleOwnerModel?> GetVehicleOwnerForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.VehicleOwners
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new VehicleOwnerModel
            {
                Id = o.Id,
                OwnerCode = o.OwnerCode,
                PANType = o.PANType,
                PANNumber = o.PANNumber,
                FirstName = o.FirstName,
                MiddleName = o.MiddleName,
                LastName = o.LastName,
                CompanyName = o.CompanyName,
                City = o.City,
                State = o.State,
                Address = o.Address,
                Phone = o.Phone,
                Mobile = o.Mobile,
                Email = o.Email,
                Fax = o.Fax,
                IsActive = o.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<VehicleOwnerListItem> ProjectToListItem(IQueryable<VehicleOwnerEntity> query)
    {
        return query.Select(o => new VehicleOwnerListItem
        {
            Id = o.Id,
            OwnerCode = o.OwnerCode,
            PANType = o.PANType,
            PANNumber = o.PANNumber,
            FirstName = o.FirstName,
            LastName = o.LastName,
            CompanyName = o.CompanyName,
            Mobile = o.Mobile,
            City = o.City,
            State = o.State,
            IsActive = o.IsActive
        });
    }
}
