using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using veteran_logistic.FinancialYear.Contracts;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;

namespace veteran_logistic.Transactions.UnloadingRegisters.Services;

/// <summary>
/// Implementation of the unloading register query service.
/// </summary>
public sealed class UnloadingRegisterQueryService : IUnloadingRegisterQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<UnloadingRegisterQueryService> _logger;
    private readonly IFinancialYearContext _financialYearContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnloadingRegisterQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="financialYearContext">The financial year context.</param>
    public UnloadingRegisterQueryService(VeteranLogisticsDbContext dbContext, ILogger<UnloadingRegisterQueryService> logger, IFinancialYearContext financialYearContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _financialYearContext = financialYearContext ?? throw new ArgumentNullException(nameof(financialYearContext));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnloadingRegisterListItem>> GetAllUnloadingRegistersAsync(CancellationToken cancellationToken = default)
    {
        var query = ApplyFinancialYearFilter(_dbContext.UnloadingRegisters.AsNoTracking());
        return await ProjectToListItem(query)
            .OrderBy(ur => ur.UnloadingDate)
            .ThenBy(ur => ur.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnloadingRegisterListItem>> SearchUnloadingRegistersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = ApplyFinancialYearFilter(_dbContext.UnloadingRegisters.AsNoTracking());

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(ur =>
                EF.Functions.Like(ur.ChallanNumber, searchPattern) ||
                EF.Functions.Like(ur.TPNumber, searchPattern) ||
                (ur.Vehicle != null && EF.Functions.Like(ur.Vehicle.VehicleNumber, searchPattern)) ||
                (ur.Consignor != null && EF.Functions.Like(ur.Consignor.CustomerName, searchPattern)) ||
                (ur.Consignee != null && EF.Functions.Like(ur.Consignee.CustomerName, searchPattern)) ||
                EF.Functions.Like(ur.Driver, searchPattern) ||
                (ur.Material != null && EF.Functions.Like(ur.Material.MaterialName, searchPattern)));
        }

        return await ProjectToListItem(query)
            .OrderBy(ur => ur.UnloadingDate)
            .ThenBy(ur => ur.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<UnloadingRegisterModel?> GetUnloadingRegisterForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => ur.Id == id)
            .Select(ur => new UnloadingRegisterModel
            {
                Id = ur.Id,
                ChallanNumber = ur.ChallanNumber,
                LoadingRegisterId = ur.LoadingRegisterId,
                LoadingRegisterChallanNumber = ur.LoadingRegister != null ? ur.LoadingRegister.ChallanNumber : null,
                ConsignorId = ur.ConsignorId,
                ConsigneeId = ur.ConsigneeId,
                SourceId = ur.SourceId,
                DestinationId = ur.DestinationId,
                UnloadingDate = ur.UnloadingDate,
                TPNumber = ur.TPNumber,
                VehicleId = ur.VehicleId,
                VehicleType = ur.VehicleType,
                UnionVendorId = ur.UnionVendorId,
                DriverCommission = ur.DriverCommission,
                GrossWeight = ur.GrossWeight,
                TareWeight = ur.TareWeight,
                LoadingWeight = ur.LoadingWeight,
                GrossWeightUL = ur.GrossWeightUL,
                TareWeightUL = ur.TareWeightUL,
                UnloadingWeight = ur.UnloadingWeight,
                ChallanMoney = ur.ChallanMoney,
                MaterialId = ur.MaterialId,
                Rate = ur.Rate,
                GrossAmount = ur.GrossAmount,
                VehicleLoadedBy = ur.VehicleLoadedBy,
                FuelQuantity = ur.FuelQuantity,
                FuelAmount = ur.FuelAmount,
                FuelCash = ur.FuelCash,
                FuelAdvance = ur.FuelAdvance,
                ShortageWeight = ur.ShortageWeight,
                CashAdvance = ur.CashAdvance,
                PaymentLocationId = ur.PaymentLocationId,
                OtherAdvance = ur.OtherAdvance,
                OtherAdvanceDate = ur.OtherAdvanceDate,
                ThirdParty = ur.ThirdParty,
                OwnerId = ur.OwnerId,
                OwnerMobile = ur.OwnerMobile,
                OwnerAddress = ur.OwnerAddress,
                Driver = ur.Driver,
                DrivingLicenceNumber = ur.DrivingLicenceNumber,
                DriverMobile = ur.DriverMobile,
                Notes = ur.Notes,
                IsActive = ur.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<UnloadingRegisterListItem> ProjectToListItem(IQueryable<UnloadingRegisterEntity> query)
    {
        return query.Select(ur => new UnloadingRegisterListItem
        {
            Id = ur.Id,
            ChallanNumber = ur.ChallanNumber,
            LoadingRegisterId = ur.LoadingRegisterId,
            LoadingRegisterChallanNumber = ur.LoadingRegister != null ? ur.LoadingRegister.ChallanNumber : null,
            UnloadingDate = ur.UnloadingDate,
            TPNumber = ur.TPNumber,
            VehicleNumber = ur.Vehicle != null ? ur.Vehicle.VehicleNumber : null,
            ConsignorName = ur.Consignor != null ? ur.Consignor.CustomerName : null,
            ConsigneeName = ur.Consignee != null ? ur.Consignee.CustomerName : null,
            SourceName = ur.Source != null ? ur.Source.LocationName : null,
            DestinationName = ur.Destination != null ? ur.Destination.LocationName : null,
            MaterialName = ur.Material != null ? ur.Material.MaterialName : null,
            Driver = ur.Driver,
            LoadingWeight = ur.LoadingWeight,
            GrossWeightUL = ur.GrossWeightUL,
            TareWeightUL = ur.TareWeightUL,
            UnloadingWeight = ur.UnloadingWeight,
            ChallanMoney = ur.ChallanMoney,
            GrossAmount = ur.GrossAmount,
            IsActive = ur.IsActive
        });
    }

    private IQueryable<UnloadingRegisterEntity> ApplyFinancialYearFilter(IQueryable<UnloadingRegisterEntity> query)
    {
        var selectedFY = _financialYearContext.SelectedFinancialYear;
        if (selectedFY != null)
        {
            query = query.Where(ur => 
                ur.UnloadingDate >= selectedFY.StartDate && 
                ur.UnloadingDate <= selectedFY.EndDate);
        }
        return query;
    }
}
