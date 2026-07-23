using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;

namespace veteran_logistic.Transactions.LoadingRegisters.Services;

/// <summary>
/// Implementation of the loading register query service.
/// </summary>
public sealed class LoadingRegisterQueryService : ILoadingRegisterQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<LoadingRegisterQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingRegisterQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public LoadingRegisterQueryService(VeteranLogisticsDbContext dbContext, ILogger<LoadingRegisterQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LoadingRegisterListItem>> GetAllLoadingRegistersAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.LoadingRegisters.AsNoTracking())
            .OrderBy(lr => lr.LoadingDate)
            .ThenBy(lr => lr.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<LoadingRegisterListItem>> SearchLoadingRegistersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.LoadingRegisters.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(lr =>
                EF.Functions.Like(lr.ChallanNumber, searchPattern) ||
                EF.Functions.Like(lr.TPNumber, searchPattern) ||
                (lr.Vehicle != null && EF.Functions.Like(lr.Vehicle.VehicleNumber, searchPattern)) ||
                (lr.Consignor != null && EF.Functions.Like(lr.Consignor.CustomerName, searchPattern)) ||
                (lr.Consignee != null && EF.Functions.Like(lr.Consignee.CustomerName, searchPattern)) ||
                EF.Functions.Like(lr.Driver, searchPattern) ||
                (lr.Material != null && EF.Functions.Like(lr.Material.MaterialName, searchPattern)));
        }

        return await ProjectToListItem(query)
            .OrderBy(lr => lr.LoadingDate)
            .ThenBy(lr => lr.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<LoadingRegisterModel?> GetLoadingRegisterForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => lr.Id == id)
            .Select(lr => new LoadingRegisterModel
            {
                Id = lr.Id,
                ChallanNumber = lr.ChallanNumber,
                ConsignorId = lr.ConsignorId,
                ConsigneeId = lr.ConsigneeId,
                SourceId = lr.SourceId,
                DestinationId = lr.DestinationId,
                LoadingDate = lr.LoadingDate,
                TPNumber = lr.TPNumber,
                VehicleId = lr.VehicleId,
                VehicleType = lr.VehicleType,
                UnionVendorId = lr.UnionVendorId,
                DriverCommission = lr.DriverCommission,
                GrossWeight = lr.GrossWeight,
                TareWeight = lr.TareWeight,
                LoadingWeight = lr.LoadingWeight,
                MaterialId = lr.MaterialId,
                Rate = lr.Rate,
                GrossAmount = lr.GrossAmount,
                VehicleLoadedBy = lr.VehicleLoadedBy,
                FuelQuantity = lr.FuelQuantity,
                FuelAmount = lr.FuelAmount,
                FuelCash = lr.FuelCash,
                FuelAdvance = lr.FuelAdvance,
                ShortageWeight = lr.ShortageWeight,
                CashAdvance = lr.CashAdvance,
                PaymentLocationId = lr.PaymentLocationId,
                OtherAdvance = lr.OtherAdvance,
                OtherAdvanceDate = lr.OtherAdvanceDate,
                ThirdParty = lr.ThirdParty,
                OwnerId = lr.OwnerId,
                OwnerMobile = lr.OwnerMobile,
                OwnerAddress = lr.OwnerAddress,
                Driver = lr.Driver,
                DrivingLicenceNumber = lr.DrivingLicenceNumber,
                DriverMobile = lr.DriverMobile,
                Notes = lr.Notes,
                IsActive = lr.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<LoadingRegisterListItem> ProjectToListItem(IQueryable<LoadingRegisterEntity> query)
    {
        return query.Select(lr => new LoadingRegisterListItem
        {
            Id = lr.Id,
            ChallanNumber = lr.ChallanNumber,
            LoadingDate = lr.LoadingDate,
            TPNumber = lr.TPNumber,
            VehicleNumber = lr.Vehicle != null ? lr.Vehicle.VehicleNumber : null,
            ConsignorName = lr.Consignor != null ? lr.Consignor.CustomerName : null,
            ConsigneeName = lr.Consignee != null ? lr.Consignee.CustomerName : null,
            SourceName = lr.Source != null ? lr.Source.LocationName : null,
            DestinationName = lr.Destination != null ? lr.Destination.LocationName : null,
            MaterialName = lr.Material != null ? lr.Material.MaterialName : null,
            Driver = lr.Driver,
            LoadingWeight = lr.LoadingWeight,
            GrossAmount = lr.GrossAmount,
            IsActive = lr.IsActive
        });
    }
}
