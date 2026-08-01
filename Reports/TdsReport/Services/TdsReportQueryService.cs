using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using veteran_logistic.Reports.TdsReport.Contracts;
using veteran_logistic.Reports.TdsReport.DTOs;

namespace veteran_logistic.Reports.TdsReport.Services;

/// <summary>
/// Implementation of the TDS report query service.
/// </summary>
public sealed class TdsReportQueryService : ITdsReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<TdsReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TdsReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public TdsReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<TdsReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<TdsReportItem> Items, TdsReportTotals Totals)> GetTdsReportAsync(
        TdsReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating TDS report with filters and search");

        var query = _dbContext.PaymentRegisters
            .AsNoTracking()
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Consignor)
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Vehicle)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Consignor)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Vehicle)
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

        _logger.LogInformation("TDS report generated successfully with {Count} records", items.Count);

        return (items, totals);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TdsReportGroupSummary>> GetGroupedSummaryAsync(
        string groupBy,
        TdsReportFilter filter,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating TDS report grouped summary by {GroupBy}", groupBy);

        var query = _dbContext.PaymentRegisters
            .AsNoTracking()
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Consignor)
            .Include(pr => pr.LoadingRegister)
                .ThenInclude(lr => lr!.Vehicle)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Consignor)
            .Include(pr => pr.UnloadingRegister)
                .ThenInclude(ur => ur!.Vehicle)
            .Include(pr => pr.PaymentLocation)
            .Where(pr => !pr.IsDeleted);

        // Apply filters
        query = ApplyFilters(query, filter);

        // Apply grouping based on the groupBy parameter
        var groupedQuery = groupBy.ToLower() switch
        {
            "customer" => query
                .GroupBy(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName :
                           pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : "Unknown"),
            "beneficiary" => query.GroupBy(pr => pr.Beneficiary),
            "pan" => query.GroupBy(pr => pr.PAN),
            "bank" => query.GroupBy(pr => pr.BankName),
            "paymenttype" => query.GroupBy(pr => pr.PaymentType),
            "tdspercentage" => query.GroupBy(pr => pr.TDSPercentage.ToString()),
            _ => query.GroupBy(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName :
                           pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : "Unknown")
        };

        var summaries = await groupedQuery
            .Select(g => new TdsReportGroupSummary
            {
                GroupKey = g.Key,
                TransactionCount = g.Count(),
                TotalChallanAmount = g.Sum(pr => pr.ChallanMoney),
                TotalTDSAmount = g.Sum(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
                AverageTDS = g.Sum(pr => pr.PayableAmount * (pr.TDSPercentage / 100)) / g.Count()
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("TDS grouped summary generated successfully with {Count} groups", summaries.Count);

        return summaries;
    }

    private static IQueryable<PaymentRegisterEntity> ApplyFilters(IQueryable<PaymentRegisterEntity> query, TdsReportFilter filter)
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

        if (!string.IsNullOrWhiteSpace(filter.Driver))
        {
            query = query.Where(pr => 
                (pr.LoadingRegister != null && EF.Functions.Like(pr.LoadingRegister.Driver, $"%{filter.Driver}%")) ||
                (pr.UnloadingRegister != null && EF.Functions.Like(pr.UnloadingRegister.Driver, $"%{filter.Driver}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.Beneficiary))
        {
            query = query.Where(pr => EF.Functions.Like(pr.Beneficiary, $"%{filter.Beneficiary}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.PAN))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PAN, $"%{filter.PAN}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.BankName))
        {
            query = query.Where(pr => EF.Functions.Like(pr.BankName, $"%{filter.BankName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.PaymentType))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PaymentType, $"%{filter.PaymentType}%"));
        }

        if (filter.PaymentLocationId.HasValue)
        {
            query = query.Where(pr => pr.PaymentLocationId == filter.PaymentLocationId.Value);
        }

        if (filter.TDSPercentage.HasValue)
        {
            query = query.Where(pr => pr.TDSPercentage == filter.TDSPercentage.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.PaymentStatus))
        {
            query = query.Where(pr => EF.Functions.Like(pr.PaymentStatus, $"%{filter.PaymentStatus}%"));
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
            (pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null && EF.Functions.Like(pr.LoadingRegister.Consignor.CustomerName, searchPattern)) ||
            (pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null && EF.Functions.Like(pr.UnloadingRegister.Consignor.CustomerName, searchPattern)) ||
            (pr.LoadingRegister != null && EF.Functions.Like(pr.LoadingRegister.Driver, searchPattern)) ||
            (pr.UnloadingRegister != null && EF.Functions.Like(pr.UnloadingRegister.Driver, searchPattern)) ||
            EF.Functions.Like(pr.Beneficiary, searchPattern) ||
            EF.Functions.Like(pr.PAN, searchPattern) ||
            EF.Functions.Like(pr.BankName, searchPattern) ||
            EF.Functions.Like(pr.AccountNumber, searchPattern) ||
            EF.Functions.Like(pr.IFSCCode, searchPattern) ||
            EF.Functions.Like(pr.VehicleNumber ?? "", searchPattern) ||
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
            "customer" => sortAscending
                ? query.OrderBy(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName : 
                               pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : "")
                : query.OrderByDescending(pr => pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName : 
                                     pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : ""),
            "beneficiary" => sortAscending
                ? query.OrderBy(pr => pr.Beneficiary)
                : query.OrderByDescending(pr => pr.Beneficiary),
            "pan" => sortAscending
                ? query.OrderBy(pr => pr.PAN)
                : query.OrderByDescending(pr => pr.PAN),
            "bankname" => sortAscending
                ? query.OrderBy(pr => pr.BankName)
                : query.OrderByDescending(pr => pr.BankName),
            "tdspercentage" => sortAscending
                ? query.OrderBy(pr => pr.TDSPercentage)
                : query.OrderByDescending(pr => pr.TDSPercentage),
            "tdsamount" => sortAscending
                ? query.OrderBy(pr => pr.PayableAmount * (pr.TDSPercentage / 100))
                : query.OrderByDescending(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
            "netpayment" => sortAscending
                ? query.OrderBy(pr => pr.PayableAmount)
                : query.OrderByDescending(pr => pr.PayableAmount),
            _ => query.OrderBy(pr => pr.PaymentDate).ThenBy(pr => pr.ChallanNumber)
        };
    }

    private static IQueryable<TdsReportItem> ProjectToReportItem(IQueryable<PaymentRegisterEntity> query)
    {
        return query.Select(pr => new TdsReportItem
        {
            Id = pr.Id,
            PaymentDate = pr.PaymentDate,
            ChallanNumber = pr.ChallanNumber,
            Customer = pr.LoadingRegister != null && pr.LoadingRegister.Consignor != null ? pr.LoadingRegister.Consignor.CustomerName :
                     pr.UnloadingRegister != null && pr.UnloadingRegister.Consignor != null ? pr.UnloadingRegister.Consignor.CustomerName : null,
            VehicleNumber = pr.VehicleNumber,
            Driver = pr.LoadingRegister != null ? pr.LoadingRegister.Driver :
                     pr.UnloadingRegister != null ? pr.UnloadingRegister.Driver : string.Empty,
            Beneficiary = pr.Beneficiary,
            PAN = pr.PAN,
            BankName = pr.BankName,
            PaymentType = pr.PaymentType,
            ChallanAmount = pr.ChallanMoney,
            TDSPercentage = pr.TDSPercentage,
            TDSAmount = pr.PayableAmount * (pr.TDSPercentage / 100),
            Surcharge = pr.Surcharge,
            AdminCharge = pr.AdminCharge,
            NetPayment = pr.PayableAmount,
            PaymentStatus = pr.PaymentStatus
        });
    }

    private static async Task<TdsReportTotals> CalculateTotalsAsync(IQueryable<PaymentRegisterEntity> query, CancellationToken cancellationToken)
    {
        var totals = await query
            .GroupBy(pr => 1)
            .Select(g => new TdsReportTotals
            {
                RecordCount = g.Count(),
                TotalChallanAmount = g.Sum(pr => pr.ChallanMoney),
                TotalTDSAmount = g.Sum(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
                TotalSurcharge = g.Sum(pr => pr.Surcharge),
                TotalAdminCharge = g.Sum(pr => pr.AdminCharge),
                TotalNetPayment = g.Sum(pr => pr.PayableAmount),
                AverageTDSAmount = g.Count() > 0 ? g.Sum(pr => pr.PayableAmount * (pr.TDSPercentage / 100)) / g.Count() : 0,
                HighestTDSAmount = g.Max(pr => pr.PayableAmount * (pr.TDSPercentage / 100)),
                LowestTDSAmount = g.Min(pr => pr.PayableAmount * (pr.TDSPercentage / 100))
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return totals ?? new TdsReportTotals();
    }
}
