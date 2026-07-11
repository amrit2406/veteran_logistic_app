using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VendorEntity = VeteranLogistics.Data.Entities.Administration.Vendor;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;

namespace veteran_logistic.Masters.Vendors.Services;

/// <summary>
/// Implementation of the vendor query service.
/// </summary>
public sealed class VendorQueryService : IVendorQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<VendorQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public VendorQueryService(VeteranLogisticsDbContext dbContext, ILogger<VendorQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VendorListItem>> GetAllVendorsAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Vendors.AsNoTracking())
            .OrderBy(v => v.VendorName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VendorListItem>> SearchVendorsAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Vendors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(v =>
                EF.Functions.Like(v.VendorCode, searchPattern) ||
                EF.Functions.Like(v.VendorName, searchPattern) ||
                EF.Functions.Like(v.City, searchPattern) ||
                EF.Functions.Like(v.State, searchPattern));
        }

        return await ProjectToListItem(query)
            .OrderBy(v => v.VendorName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<VendorModel?> GetVendorForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Vendors
            .AsNoTracking()
            .Where(v => v.Id == id)
            .Select(v => new VendorModel
            {
                Id = v.Id,
                VendorCode = v.VendorCode,
                VendorName = v.VendorName,
                AddressLine1 = v.AddressLine1,
                AddressLine2 = v.AddressLine2,
                City = v.City,
                State = v.State,
                Country = v.Country,
                PostalCode = v.PostalCode,
                PhoneNumber = v.PhoneNumber,
                Email = v.Email,
                GSTNumber = v.GSTNumber,
                PANNumber = v.PANNumber,
                ContactPerson = v.ContactPerson,
                IsActive = v.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<VendorListItem> ProjectToListItem(IQueryable<VendorEntity> query)
    {
        return query.Select(v => new VendorListItem
        {
            Id = v.Id,
            VendorCode = v.VendorCode,
            VendorName = v.VendorName,
            GSTNumber = v.GSTNumber,
            PANNumber = v.PANNumber,
            City = v.City,
            State = v.State,
            IsActive = v.IsActive
        });
    }
}
