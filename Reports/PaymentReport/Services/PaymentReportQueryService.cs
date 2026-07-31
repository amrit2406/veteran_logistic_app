using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using veteran_logistic.Reports.PaymentReport.Contracts;
using veteran_logistic.Reports.PaymentReport.DTOs;

namespace veteran_logistic.Reports.PaymentReport.Services;

/// <summary>
/// Implementation of the payment report query service.
/// </summary>
public sealed class PaymentReportQueryService : IPaymentReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PaymentReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PaymentReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<PaymentReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<PaymentReportItem> Items, PaymentReportTotals Totals)> GetPaymentReportAsync(
        PaymentReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating payment report with filters and search");

        var query = _dbContext.PaymentRegisters
            .AsNoTracking()
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Consignor)
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Vehicle)
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Material)
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Owner)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Consignor)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Vehicle)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Material)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Owner)
            .Include(pr => pr.PaymentLocation)
            .Where(pr => !pr.IsDeleted);

        // Apply filters
        query = ApplyFilters(query, filter);

        // Apply search
        query = ApplySearch(query, search);

        // Calculate totals before pagination
        var totalsQuery = query;
        var totals = await CalculateTotalsAsync(totalsQuery, cancellationToken);

        // Apply sorting
        query = ApplySorting(query, sortBy, sortAscending);

        // Project to DTO
        var items = await ProjectToReportItem(query)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Payment report generated successfully with {Count} records", items.Count);

        return (items, totals);
    }

    private static IQueryable<PaymentRegisterEntity> ApplyFilters(IQueryable<PaymentRegisterEntity> query, PaymentReportFilter filter)
    {
        if (filter.DateFrom.HasValue)
        {
            query = query.Where(pr => pr.PaymentDate >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            query = query.Where(pr => pr.PaymentDate <= filter.DateTo.Value);
        }

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && pr.LoadingRegister.ConsignorId == filter.CustomerId.Value) ||
                (pr.UnloadingRegister != null && pr.UnloadingRegister.ConsignorId == filter.CustomerId.Value));
        }

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && pr.LoadingRegister.VehicleId == filter.VehicleId.Value) ||
                (pr.UnloadingRegister != null && pr.UnloadingRegister.VehicleId == filter.VehicleId.Value));
        }

        if (filter.MaterialId.HasValue)
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && pr.LoadingRegister.MaterialId == filter.MaterialId.Value) ||
                (pr.UnloadingRegister != null && pr.UnloadingRegister.MaterialId == filter.MaterialId.Value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Driver))
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && EF.Functions.Like(pr.LoadingRegister.Driver, $"%{filter.Driver}%")) ||
                (pr.UnloadingRegister != null && EF.Functions.Like(pr.UnloadingRegister.Driver, $"%{filter.Driver}%")));
        }

        if (filter.OwnerId.HasValue)
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && pr.LoadingRegister.OwnerId == filter.OwnerId.Value) ||
                (pr.UnloadingRegister != null && pr.UnloadingRegister.OwnerId == filter.OwnerId.Value));
        }

        if (filter.PaymentLocationId.HasValue)
        {
            query = query.Where(pr => pr.PaymentLocationId == filter.PaymentLocationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.PaymentType))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PaymentType, $"%{filter.PaymentType}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Beneficiary))
        {
            query = query.Where(pr => EF.Functions.Like(pr.Beneficiary, $"%{filter.Beneficiary}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.BankName))
        {
            query = query.Where(pr => EF.Functions.Like(pr.BankName, $"%{filter.BankName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.ChallanNumber))
        {
            query = query.Where(pr => EF.Functions.Like(pr.ChallanNumber, $"%{filter.ChallanNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.TPNumber))
        {
            query = query.Where(pr => EF.Functions.Like(pr.TPNumber, $"%{filter.TPNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.UTRNumber))
        {
            query = query.Where(pr => EF.Functions.Like(pr.UTRNumber, $"%{filter.UTRNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.PAN))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PAN, $"%{filter.PAN}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PaymentStatus, $"%{filter.PaymentStatus}%"));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(pr => pr.IsActive == filter.IsActive.Value);
        }

        return query;
    }

    private static IQueryable<PaymentRegisterEntity> ApplySearch(IQueryable<PaymentRegisterEntity> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var searchPattern = $"%{search}%";
        return query.Where(pr =>
            EF.Functions.Like(pr.ChallanNumber, searchPattern) ||
            EF.Functions.Like(pr.TPNumber, searchPattern) ||
            EF.Functions.Like(pr.VehicleNumber ?? "", searchPattern) ||
            (pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null && EF.Functions.Like(pr.LoadingRegister.Consignor.CustomerName, searchPattern)) ||
            (pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null && EF.Functions.Like(pr.UnloadingRegister.Consignor.CustomerName, searchPattern)) ||
            (pr.LoadingRegister != null && EF.Functions.Like(pr.LoadingRegister.Driver, searchPattern)) ||
            (pr.UnloadingRegister != null && EF.Functions.Like(pr.UnloadingRegister.Driver, searchPattern)) ||
            EF.Functions.Like(pr.Beneficiary, searchPattern) ||
            EF.Functions.Like(pr.PAN, searchPattern) ||
            EF.Functions.Like(pr.BankName, searchPattern) ||
            EF.Functions.Like(pr.AccountNumber, searchPattern) ||
            EF.Functions.Like(pr.IFSCCode, searchPattern) ||
            EF.Functions.Like(pr.UTRNumber, searchPattern));
    }

    private static IQueryable<PaymentRegisterEntity> ApplySorting(IQueryable<PaymentRegisterEntity> query, string? sortBy, bool sortAscending)
    {
        return (sortBy?.ToLower()) switch
        {
            "paymentdate" => sortAscending
                ? query.OrderBy(pr => pr.PaymentDate).ThenBy(pr => pr.ChallanNumber)
                : query.OrderByDescending(pr => pr.PaymentDate).ThenByDescending(pr => pr.ChallanNumber),
            "challannumber" => sortAscending
                ? query.OrderBy(pr => pr.ChallanNumber)
                : query.OrderByDescending(pr => pr.ChallanNumber),
            "vehicle" => sortAscending
                ? query.OrderBy(pr => pr.VehicleNumber ?? "")
                : query.OrderByDescending(pr => pr.VehicleNumber ?? ""),
            "customer" => sortAscending
                ? query.OrderBy(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName : 
                               pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : "")
                : query.OrderByDescending(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName : 
                                     pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : ""),
            "paymenttype" => sortAscending
                ? query.OrderBy(pr => pr.PaymentType)
                : query.OrderByDescending(pr => pr.PaymentType),
            "beneficiary" => sortAscending
                ? query.OrderBy(pr => pr.Beneficiary)
                : query.OrderByDescending(pr => pr.Beneficiary),
            "bankname" => sortAscending
                ? query.OrderBy(pr => pr.BankName)
                : query.OrderByDescending(pr => pr.BankName),
            "drivercommission" => sortAscending
                ? query.OrderBy(pr => pr.DriverCommission)
                : query.OrderByDescending(pr => pr.DriverCommission),
            "challanamount" => sortAscending
                ? query.OrderBy(pr => pr.ChallanMoney)
                : query.OrderByDescending(pr => pr.ChallanMoney),
            "tdsamount" => sortAscending
                ? query.OrderBy(pr => pr.PayableAmount * (pr.TDSPercentage / 100))
                : query.OrderByDescending(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
            "netpayment" => sortAscending
                ? query.OrderBy(pr => pr.PayableAmount)
                : query.OrderByDescending(pr => pr.PayableAmount),
            _ => query.OrderBy(pr => pr.PaymentDate).ThenBy(pr => pr.ChallanNumber)
        };
    }

    private static IQueryable<PaymentReportItem> ProjectToReportItem(IQueryable<PaymentRegisterEntity> query)
    {
        return query.Select(pr => new PaymentReportItem
        {
            Id = pr.Id,
            PaymentDate = pr.PaymentDate,
            ChallanNumber = pr.ChallanNumber,
            TPNumber = pr.TPNumber,
            VehicleNumber = pr.VehicleNumber,
            LoadingDate = pr.LoadingDate,
            UnloadingDate = pr.UnloadingDate,
            CustomerName = pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName :
                           pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : null,
            MaterialName = pr.LoadingRegister != null && pr.LoadingRegister.Material != null ? pr.LoadingRegister.Material.MaterialName :
                           pr.UnloadingRegister != null && pr.UnloadingRegister.Material != null ? pr.UnloadingRegister.Material.MaterialName : pr.MaterialName,
            Driver = pr.LoadingRegister != null ? pr.LoadingRegister.Driver :
                     pr.UnloadingRegister != null ? pr.UnloadingRegister.Driver : string.Empty,
            VehicleOwner = pr.LoadingRegister != null && pr.LoadingRegister.Owner != null ? 
                           (!string.IsNullOrWhiteSpace(pr.LoadingRegister.Owner.CompanyName) ? pr.LoadingRegister.Owner.CompanyName : $"{pr.LoadingRegister.Owner.FirstName} {pr.LoadingRegister.Owner.LastName}".Trim()) :
                           pr.UnloadingRegister != null && pr.UnloadingRegister.Owner != null ?
                           (!string.IsNullOrWhiteSpace(pr.UnloadingRegister.Owner.CompanyName) ? pr.UnloadingRegister.Owner.CompanyName : $"{pr.UnloadingRegister.Owner.FirstName} {pr.UnloadingRegister.Owner.LastName}".Trim()) : null,
            PaymentLocationName = pr.PaymentLocation != null ? pr.PaymentLocation.PaymentLocationName : null,
            PaymentType = pr.PaymentType,
            Beneficiary = pr.Beneficiary,
            PAN = pr.PAN,
            BankName = pr.BankName,
            AccountNumber = pr.AccountNumber,
            IFSCCode = pr.IFSCCode,
            UTRNumber = pr.UTRNumber,
            MobileNumber = pr.MobileNumber,
            LoadingWeight = pr.LoadingWeight,
            UnloadingWeight = pr.UnloadingWeight,
            DriverCommission = pr.DriverCommission,
            ChallanAmount = pr.ChallanMoney,
            TDSAmount = pr.PayableAmount * (pr.TDSPercentage / 100),
            SurchargeAmount = pr.Surcharge,
            AdminCharge = pr.AdminCharge,
            NetPayment = pr.PayableAmount,
            Notes = pr.Notes,
            PaymentStatus = pr.PaymentStatus,
            IsActive = pr.IsActive
        });
    }

    private static async Task<PaymentReportTotals> CalculateTotalsAsync(IQueryable<PaymentRegisterEntity> query, CancellationToken cancellationToken)
    {
        var totals = await query
            .GroupBy(pr => 1)
            .Select(g => new PaymentReportTotals
            {
                RecordCount = g.Count(),
                TotalLoadingWeight = g.Sum(pr => pr.LoadingWeight),
                TotalUnloadingWeight = g.Sum(pr => pr.UnloadingWeight),
                TotalDriverCommission = g.Sum(pr => pr.DriverCommission),
                TotalChallanAmount = g.Sum(pr => pr.ChallanMoney),
                TotalTDSAmount = g.Sum(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
                TotalSurchargeAmount = g.Sum(pr => pr.Surcharge),
                TotalAdminCharge = g.Sum(pr => pr.AdminCharge),
                TotalNetPayment = g.Sum(pr => pr.PayableAmount)
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return totals ?? new PaymentReportTotals();
    }
}
