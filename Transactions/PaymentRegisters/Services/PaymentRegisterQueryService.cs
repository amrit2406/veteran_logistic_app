using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using veteran_logistic.FinancialYear.Contracts;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;

namespace veteran_logistic.Transactions.PaymentRegisters.Services;

/// <summary>
/// Implementation of the payment register query service.
/// </summary>
public sealed class PaymentRegisterQueryService : IPaymentRegisterQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PaymentRegisterQueryService> _logger;
    private readonly IFinancialYearContext _financialYearContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentRegisterQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="financialYearContext">The financial year context.</param>
    public PaymentRegisterQueryService(VeteranLogisticsDbContext dbContext, ILogger<PaymentRegisterQueryService> logger, IFinancialYearContext financialYearContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _financialYearContext = financialYearContext ?? throw new ArgumentNullException(nameof(financialYearContext));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaymentRegisterListItem>> GetAllPaymentRegistersAsync(CancellationToken cancellationToken = default)
    {
        var query = ApplyFinancialYearFilter(_dbContext.PaymentRegisters.AsNoTracking());
        return await ProjectToListItem(query)
            .OrderBy(pr => pr.PaymentDate)
            .ThenBy(pr => pr.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PaymentRegisterListItem>> SearchPaymentRegistersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = ApplyFinancialYearFilter(_dbContext.PaymentRegisters.AsNoTracking());

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(pr =>
                EF.Functions.Like(pr.ChallanNumber, searchPattern) ||
                EF.Functions.Like(pr.TPNumber, searchPattern) ||
                EF.Functions.Like(pr.VehicleNumber ?? "", searchPattern) ||
                EF.Functions.Like(pr.MaterialName ?? "", searchPattern) ||
                EF.Functions.Like(pr.Beneficiary, searchPattern) ||
                EF.Functions.Like(pr.PaymentStatus, searchPattern));
        }

        return await ProjectToListItem(query)
            .OrderBy(pr => pr.PaymentDate)
            .ThenBy(pr => pr.ChallanNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PaymentRegisterModel?> GetPaymentRegisterForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => pr.Id == id)
            .Select(pr => new PaymentRegisterModel
            {
                Id = pr.Id,
                ChallanNumber = pr.ChallanNumber,
                LoadingRegisterId = pr.LoadingRegisterId,
                UnloadingRegisterId = pr.UnloadingRegisterId,
                TPNumber = pr.TPNumber,
                VehicleNumber = pr.VehicleNumber,
                VehicleType = pr.VehicleType,
                MaterialName = pr.MaterialName,
                DriverCommission = pr.DriverCommission,
                LoadingDate = pr.LoadingDate,
                UnloadingDate = pr.UnloadingDate,
                LoadingWeight = pr.LoadingWeight,
                UnloadingWeight = pr.UnloadingWeight,
                PaymentDate = pr.PaymentDate,
                PaymentLocationId = pr.PaymentLocationId,
                PaymentType = pr.PaymentType,
                HSDParty = pr.HSDParty,
                Notes = pr.Notes,
                Beneficiary = pr.Beneficiary,
                PAN = pr.PAN,
                UTRNumber = pr.UTRNumber,
                MobileNumber = pr.MobileNumber,
                AccountNumber = pr.AccountNumber,
                IFSCCode = pr.IFSCCode,
                BankName = pr.BankName,
                TDSPercentage = pr.TDSPercentage,
                ChallanMoney = pr.ChallanMoney,
                Surcharge = pr.Surcharge,
                AdminCharge = pr.AdminCharge,
                GrossAmount = pr.GrossAmount,
                PayableAmount = pr.PayableAmount,
                PaymentStatus = pr.PaymentStatus,
                IsActive = pr.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PaymentRegisterModel?> GetPaymentRegisterByChallanNumberAsync(string challanNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => pr.ChallanNumber == challanNumber)
            .Select(pr => new PaymentRegisterModel
            {
                Id = pr.Id,
                ChallanNumber = pr.ChallanNumber,
                LoadingRegisterId = pr.LoadingRegisterId,
                UnloadingRegisterId = pr.UnloadingRegisterId,
                TPNumber = pr.TPNumber,
                VehicleNumber = pr.VehicleNumber,
                VehicleType = pr.VehicleType,
                MaterialName = pr.MaterialName,
                DriverCommission = pr.DriverCommission,
                LoadingDate = pr.LoadingDate,
                UnloadingDate = pr.UnloadingDate,
                LoadingWeight = pr.LoadingWeight,
                UnloadingWeight = pr.UnloadingWeight,
                PaymentDate = pr.PaymentDate,
                PaymentLocationId = pr.PaymentLocationId,
                PaymentType = pr.PaymentType,
                HSDParty = pr.HSDParty,
                Notes = pr.Notes,
                Beneficiary = pr.Beneficiary,
                PAN = pr.PAN,
                UTRNumber = pr.UTRNumber,
                MobileNumber = pr.MobileNumber,
                AccountNumber = pr.AccountNumber,
                IFSCCode = pr.IFSCCode,
                BankName = pr.BankName,
                TDSPercentage = pr.TDSPercentage,
                ChallanMoney = pr.ChallanMoney,
                Surcharge = pr.Surcharge,
                AdminCharge = pr.AdminCharge,
                GrossAmount = pr.GrossAmount,
                PayableAmount = pr.PayableAmount,
                PaymentStatus = pr.PaymentStatus,
                IsActive = pr.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<PaymentRegisterModel?> GetPaymentRegisterDataByChallanNumberAsync(string challanNumber, CancellationToken cancellationToken = default)
    {
        // Get data from Loading Register
        var loadingRegister = await _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => lr.ChallanNumber == challanNumber)
            .Select(lr => new
            {
                lr.Id,
                lr.ChallanNumber,
                lr.TPNumber,
                lr.VehicleId,
                lr.VehicleType,
                lr.DriverCommission,
                lr.LoadingDate,
                lr.LoadingWeight,
                lr.MaterialId,
                lr.GrossAmount,
                VehicleNumber = lr.Vehicle != null ? lr.Vehicle.VehicleNumber : null,
                MaterialName = lr.Material != null ? lr.Material.MaterialName : null
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (loadingRegister is null)
        {
            _logger.LogWarning("Loading register not found for challan number '{ChallanNumber}'", challanNumber);
            return null;
        }

        // Get data from Unloading Register
        var unloadingRegister = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => ur.ChallanNumber == challanNumber)
            .Select(ur => new
            {
                ur.Id,
                ur.UnloadingDate,
                ur.UnloadingWeight,
                ur.ChallanMoney
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (unloadingRegister is null)
        {
            _logger.LogWarning("Unloading register not found for challan number '{ChallanNumber}'", challanNumber);
            return null;
        }

        // Check if payment already exists
        var existingPayment = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => pr.ChallanNumber == challanNumber)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (existingPayment is not null)
        {
            _logger.LogWarning("Payment register already exists for challan number '{ChallanNumber}'", challanNumber);
            return null;
        }

        // Build the model with auto-populated data
        var model = new PaymentRegisterModel
        {
            ChallanNumber = loadingRegister.ChallanNumber,
            LoadingRegisterId = loadingRegister.Id,
            UnloadingRegisterId = unloadingRegister.Id,
            TPNumber = loadingRegister.TPNumber,
            VehicleNumber = loadingRegister.VehicleNumber,
            VehicleType = loadingRegister.VehicleType,
            MaterialName = loadingRegister.MaterialName,
            DriverCommission = loadingRegister.DriverCommission,
            LoadingDate = loadingRegister.LoadingDate,
            UnloadingDate = unloadingRegister.UnloadingDate,
            LoadingWeight = loadingRegister.LoadingWeight,
            UnloadingWeight = unloadingRegister.UnloadingWeight,
            GrossAmount = loadingRegister.GrossAmount,
            ChallanMoney = unloadingRegister.ChallanMoney,
            PaymentDate = DateTime.UtcNow,
            PaymentType = string.Empty,
            TDSPercentage = 0,
            Surcharge = 0,
            AdminCharge = 0,
            PayableAmount = 0,
            PaymentStatus = "Pending",
            IsActive = true
        };

        return model;
    }

    private static IQueryable<PaymentRegisterListItem> ProjectToListItem(IQueryable<PaymentRegisterEntity> query)
    {
        return query.Select(pr => new PaymentRegisterListItem
        {
            Id = pr.Id,
            ChallanNumber = pr.ChallanNumber,
            PaymentDate = pr.PaymentDate,
            TPNumber = pr.TPNumber,
            VehicleNumber = pr.VehicleNumber,
            MaterialName = pr.MaterialName,
            Beneficiary = pr.Beneficiary,
            PaymentStatus = pr.PaymentStatus,
            GrossAmount = pr.GrossAmount,
            PayableAmount = pr.PayableAmount,
            IsActive = pr.IsActive
        });
    }

    private IQueryable<PaymentRegisterEntity> ApplyFinancialYearFilter(IQueryable<PaymentRegisterEntity> query)
    {
        var selectedFY = _financialYearContext.SelectedFinancialYear;
        if (selectedFY != null)
        {
            query = query.Where(pr => 
                pr.PaymentDate >= selectedFY.StartDate && 
                pr.PaymentDate <= selectedFY.EndDate);
        }
        return query;
    }
}
