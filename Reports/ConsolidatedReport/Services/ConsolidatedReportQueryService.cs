using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Reports.ConsolidatedReport.Contracts;
using veteran_logistic.Reports.ConsolidatedReport.DTOs;
using System.Linq;

namespace veteran_logistic.Reports.ConsolidatedReport.Services;

/// <summary>
/// Implementation of the consolidated report query service that joins Loading, Unloading, Payment, and Billing data.
/// </summary>
public sealed class ConsolidatedReportQueryService : IConsolidatedReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<ConsolidatedReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsolidatedReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public ConsolidatedReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<ConsolidatedReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<ConsolidatedReportItem> Items, ConsolidatedReportTotals Totals, ConsolidatedReportSummaryCards SummaryCards)> GetConsolidatedReportAsync(
        ConsolidatedReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating consolidated report with filters and search");

        // Start from LoadingRegister as the base table
        var query = _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => !lr.IsDeleted);

        // Apply filters before joins for performance
        query = ApplyFilters(query, filter);

        // Perform LEFT JOINs to get all lifecycle stages with explicit navigation property loading
        var joinedQuery = from lr in query
                         join ur in _dbContext.UnloadingRegisters.Where(ur => !ur.IsDeleted)
                             on lr.Id equals ur.LoadingRegisterId into urGroup
                         from ur in urGroup.DefaultIfEmpty()
                         join pr in _dbContext.PaymentRegisters.Where(pr => !pr.IsDeleted)
                             on lr.Id equals pr.LoadingRegisterId into prGroup
                         from pr in prGroup.DefaultIfEmpty()
                         join pbrd in _dbContext.PartyBillRegisterDetails.Where(pbrd => !pbrd.IsDeleted)
                             on lr.Id equals pbrd.LoadingRegisterId into pbrdGroup
                         from pbrd in pbrdGroup.DefaultIfEmpty()
                         join pbr in _dbContext.PartyBillRegisters.Where(pbr => !pbr.IsDeleted)
                             on pbrd.PartyBillRegisterId equals pbr.Id into pbrGroup
                         from pbr in pbrGroup.DefaultIfEmpty()
                         join party in _dbContext.Customers
                             on pbr.PartyId equals party.Id into partyGroup
                         from party in partyGroup.DefaultIfEmpty()
                         join consignor in _dbContext.Customers
                             on lr.ConsignorId equals consignor.Id into consignorGroup
                         from consignor in consignorGroup.DefaultIfEmpty()
                         join consignee in _dbContext.Customers
                             on lr.ConsigneeId equals consignee.Id into consigneeGroup
                         from consignee in consigneeGroup.DefaultIfEmpty()
                         join source in _dbContext.SourceDestinations
                             on lr.SourceId equals source.Id into sourceGroup
                         from source in sourceGroup.DefaultIfEmpty()
                         join destination in _dbContext.SourceDestinations
                             on lr.DestinationId equals destination.Id into destinationGroup
                         from destination in destinationGroup.DefaultIfEmpty()
                         join vehicle in _dbContext.Vehicles
                             on lr.VehicleId equals vehicle.Id into vehicleGroup
                         from vehicle in vehicleGroup.DefaultIfEmpty()
                         join material in _dbContext.Materials
                             on lr.MaterialId equals material.Id into materialGroup
                         from material in materialGroup.DefaultIfEmpty()
                         join owner in _dbContext.VehicleOwners
                             on lr.OwnerId equals owner.Id into ownerGroup
                         from owner in ownerGroup.DefaultIfEmpty()
                         join paymentLocation in _dbContext.PaymentLocations
                             on lr.PaymentLocationId equals paymentLocation.Id into paymentLocationGroup
                         from paymentLocation in paymentLocationGroup.DefaultIfEmpty()
                         select new ConsolidatedReportItem
                         {
                             // Loading data
                             LoadingRegisterId = lr.Id,
                             LoadingDate = lr.LoadingDate,
                             ChallanNumber = lr.ChallanNumber,
                             TPNumber = lr.TPNumber,
                             VehicleNumber = vehicle != null ? vehicle.VehicleNumber : null,
                             MaterialName = material != null ? material.MaterialName : null,
                             ConsignorName = consignor != null ? consignor.CustomerName : null,
                             ConsigneeName = consignee != null ? consignee.CustomerName : null,
                             SourceName = source != null ? source.LocationName : null,
                             DestinationName = destination != null ? destination.LocationName : null,
                             LoadingWeight = lr.LoadingWeight,
                             Rate = lr.Rate,
                             LoadingAmount = lr.GrossAmount,
                             Driver = lr.Driver,
                             OwnerName = owner != null ? $"{owner.FirstName} {owner.LastName}" : null,
                             CompanyName = null,
                             PaymentLocationName = paymentLocation != null ? paymentLocation.PaymentLocationName : null,
                             
                             // Unloading data
                             UnloadingRegisterId = ur != null ? (int?)ur.Id : null,
                             UnloadingDate = ur != null ? (DateTime?)ur.UnloadingDate : null,
                             UnloadingWeight = ur != null ? (decimal?)ur.UnloadingWeight : null,
                             ShortageWeight = ur != null ? (decimal?)ur.ShortageWeight : null,
                             
                             // Payment data
                             PaymentRegisterId = pr != null ? (int?)pr.Id : null,
                             PaymentDate = pr != null ? (DateTime?)pr.PaymentDate : null,
                             Beneficiary = pr != null ? pr.Beneficiary : null,
                             PaymentType = pr != null ? pr.PaymentType : null,
                             DriverCommission = lr.DriverCommission,
                             ChallanAmount = pr != null ? (decimal?)pr.ChallanMoney : null,
                             TDSAmount = pr != null ? (decimal?)(pr.ChallanMoney * (pr.TDSPercentage / 100)) : null,
                             Surcharge = pr != null ? (decimal?)pr.Surcharge : null,
                             AdminCharge = pr != null ? (decimal?)pr.AdminCharge : null,
                             NetPayment = pr != null ? (decimal?)pr.PayableAmount : null,
                             PaymentStatus = pr != null ? pr.PaymentStatus : null,
                             
                             // Billing data
                             PartyBillRegisterId = pbr != null ? (int?)pbr.Id : null,
                             PartyBillRegisterDetailId = pbrd != null ? (int?)pbrd.Id : null,
                             BillNumber = pbr != null ? pbr.BillNumber : null,
                             BillDate = pbr != null ? (DateTime?)pbr.BillDate : null,
                             CustomerName = party != null ? party.CustomerName : null,
                             ThirdParty = pbr != null ? pbr.ThirdPartyName : null,
                             PermitNumber = pbr != null ? pbr.PermitNumber : null,
                             BillingStatus = pbr != null ? "Billed" : null,
                             
                             // Lifecycle status calculated from actual data
                             LifecycleStatus = ur == null ? "Loading Only" : 
                                                pr == null ? "Loaded &amp; Unloaded" : 
                                                pbr == null ? "Payment Completed" : "Completed"
                         };

        // Apply search on joined data
        joinedQuery = ApplySearch(joinedQuery, search);

        // Apply SAR filter
        joinedQuery = ApplySARFilter(joinedQuery, filter);


        // Apply sorting
        joinedQuery = ApplySorting(joinedQuery, sortBy, sortAscending);

        // Materialize the data for calculations
        var items = await joinedQuery.ToListAsync(cancellationToken).ConfigureAwait(false);

        // Calculate summary cards and totals from materialized data
        var summaryCards = CalculateSummaryCards(items);
        var totals = CalculateTotals(items);

        _logger.LogInformation("Consolidated report generated successfully with {Count} records", items.Count);

        return (items, totals, summaryCards);
    }

    private static IQueryable<LoadingRegister> ApplyFilters(IQueryable<LoadingRegister> query, ConsolidatedReportFilter filter)
    {
        // Date filter
        if (filter.DateFrom.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate <= filter.DateTo.Value);
        }

        // Entity filters
        if (filter.ConsignorId.HasValue)
        {
            query = query.Where(lr => lr.ConsignorId == filter.ConsignorId.Value);
        }

        if (filter.ConsigneeId.HasValue)
        {
            query = query.Where(lr => lr.ConsigneeId == filter.ConsigneeId.Value);
        }

        if (filter.SourceId.HasValue)
        {
            query = query.Where(lr => lr.SourceId == filter.SourceId.Value);
        }

        if (filter.DestinationId.HasValue)
        {
            query = query.Where(lr => lr.DestinationId == filter.DestinationId.Value);
        }

        return query;
    }

    private static IQueryable<ConsolidatedReportItem> ApplySearch(IQueryable<ConsolidatedReportItem> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var searchPattern = $"%{search}%";
        return query.Where(item => 
            EF.Functions.Like(item.ChallanNumber, searchPattern) ||
            EF.Functions.Like(item.TPNumber, searchPattern) ||
            EF.Functions.Like(item.Driver, searchPattern) ||
            EF.Functions.Like(item.ConsignorName ?? string.Empty, searchPattern) ||
            EF.Functions.Like(item.ConsigneeName ?? string.Empty, searchPattern) ||
            EF.Functions.Like(item.SourceName ?? string.Empty, searchPattern) ||
            EF.Functions.Like(item.DestinationName ?? string.Empty, searchPattern) ||
            EF.Functions.Like(item.VehicleNumber ?? string.Empty, searchPattern) ||
            EF.Functions.Like(item.MaterialName ?? string.Empty, searchPattern));
    }

    private static IQueryable<ConsolidatedReportItem> ApplySorting(IQueryable<ConsolidatedReportItem> query, string? sortBy, bool sortAscending)
    {
        return sortBy?.ToLower() switch
        {
            "loadingdate" => sortAscending ? 
                query.OrderBy(item => item.LoadingDate) : 
                query.OrderByDescending(item => item.LoadingDate),
            "unloadingdate" => sortAscending ? 
                query.OrderBy(item => item.UnloadingDate) : 
                query.OrderByDescending(item => item.UnloadingDate),
            "paymentdate" => sortAscending ? 
                query.OrderBy(item => item.PaymentDate) : 
                query.OrderByDescending(item => item.PaymentDate),
            "billdate" => sortAscending ? 
                query.OrderBy(item => item.BillDate) : 
                query.OrderByDescending(item => item.BillDate),
            "vehicle" => sortAscending ? 
                query.OrderBy(item => item.VehicleNumber) : 
                query.OrderByDescending(item => item.VehicleNumber),
            "consignor" => sortAscending ? 
                query.OrderBy(item => item.ConsignorName) : 
                query.OrderByDescending(item => item.ConsignorName),
            "consignee" => sortAscending ? 
                query.OrderBy(item => item.ConsigneeName) : 
                query.OrderByDescending(item => item.ConsigneeName),
            "source" => sortAscending ? 
                query.OrderBy(item => item.SourceName) : 
                query.OrderByDescending(item => item.SourceName),
            "destination" => sortAscending ? 
                query.OrderBy(item => item.DestinationName) : 
                query.OrderByDescending(item => item.DestinationName),
            "billnumber" => sortAscending ? 
                query.OrderBy(item => item.BillNumber) : 
                query.OrderByDescending(item => item.BillNumber),
            "challannumber" => sortAscending ? 
                query.OrderBy(item => item.ChallanNumber) : 
                query.OrderByDescending(item => item.ChallanNumber),
            "loadingweight" => sortAscending ? 
                query.OrderBy(item => item.LoadingWeight) : 
                query.OrderByDescending(item => item.LoadingWeight),
            "unloadingweight" => sortAscending ? 
                query.OrderBy(item => item.UnloadingWeight) : 
                query.OrderByDescending(item => item.UnloadingWeight),
            "netpayment" => sortAscending ? 
                query.OrderBy(item => item.NetPayment) : 
                query.OrderByDescending(item => item.NetPayment),
            _ => query.OrderByDescending(item => item.LoadingDate)
        };
    }

    private static IQueryable<ConsolidatedReportItem> ApplySARFilter(IQueryable<ConsolidatedReportItem> query, ConsolidatedReportFilter filter)
    {
        if (string.IsNullOrWhiteSpace(filter.SARFilter))
        {
            return query;
        }

        return filter.SARFilter switch
        {
            "Show All Records" => query,
            "SAR- unloaded trips" => query.Where(item => item.UnloadingRegisterId.HasValue),
            "SAR-not unloaded trips" => query.Where(item => !item.UnloadingRegisterId.HasValue),
            "SAR-paid" => query.Where(item => item.PaymentRegisterId.HasValue),
            "SAR-unpaid" => query.Where(item => !item.PaymentRegisterId.HasValue),
            "SAR-billed" => query.Where(item => item.PartyBillRegisterId.HasValue),
            "SAR-not billed" => query.Where(item => !item.PartyBillRegisterId.HasValue),
            _ => query
        };
    }

    private static ConsolidatedReportSummaryCards CalculateSummaryCards(IReadOnlyList<ConsolidatedReportItem> items)
    {
        return new ConsolidatedReportSummaryCards
        {
            TotalTransactions = items.Count,
            LoadingOnly = items.Count(x => x.UnloadingRegisterId == null),
            PendingUnloading = items.Count(x => x.UnloadingRegisterId == null),
            PendingPayment = items.Count(x => x.UnloadingRegisterId != null && x.PaymentRegisterId == null),
            PendingBilling = items.Count(x => x.PaymentRegisterId != null && x.PartyBillRegisterId == null),
            Completed = items.Count(x => x.PartyBillRegisterId != null),
            TotalRevenue = items.Sum(x => x.LoadingAmount),
            TotalNetPayment = items.Sum(x => x.NetPayment ?? 0),
            TotalTDS = items.Sum(x => x.TDSAmount ?? 0)
        };
    }

    private static ConsolidatedReportTotals CalculateTotals(IReadOnlyList<ConsolidatedReportItem> items)
    {
        var netPayments = items.Where(x => x.NetPayment.HasValue).Select(x => x.NetPayment!.Value).ToList();
        
        return new ConsolidatedReportTotals
        {
            RecordCount = items.Count,
            TotalLoadingWeight = items.Sum(x => x.LoadingWeight),
            TotalUnloadingWeight = items.Sum(x => x.UnloadingWeight ?? 0),
            TotalShortageWeight = items.Sum(x => x.ShortageWeight ?? 0),
            TotalLoadingAmount = items.Sum(x => x.LoadingAmount),
            TotalChallanAmount = items.Sum(x => x.ChallanAmount ?? 0),
            TotalNetPayment = items.Sum(x => x.NetPayment ?? 0),
            TotalTDSAmount = items.Sum(x => x.TDSAmount ?? 0),
            TotalBills = items.Count(x => x.PartyBillRegisterId != null),
            AverageNetPayment = netPayments.Any() ? netPayments.Average() : 0
        };
    }
}
