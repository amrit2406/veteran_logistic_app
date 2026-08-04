using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using PartyBillRegisterDetailEntity = VeteranLogistics.Data.Entities.Administration.PartyBillRegisterDetail;
using veteran_logistic.Reports.DOStatusReport.Contracts;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Services;

/// <summary>
/// Implementation of the DO status report query service.
/// </summary>
public sealed class DOStatusReportQueryService : IDOStatusReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<DOStatusReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DOStatusReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public DOStatusReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<DOStatusReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<DOStatusReportItem> Items, DOStatusReportSummaryCards SummaryCards, DOStatusReportTotals Totals)> GetDOStatusReportAsync(
        DOStatusReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating DO status report with filters and search");

        var query = _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => !lr.IsDeleted);

        // Apply filters
        query = ApplyFilters(query, filter);

        // Apply search
        query = ApplySearch(query, search);

        // Calculate summary cards and totals before pagination
        var summaryCards = await CalculateSummaryCardsAsync(query, cancellationToken);
        var totals = await CalculateTotalsAsync(query, cancellationToken);

        // Apply sorting
        query = ApplySorting(query, sortBy, sortAscending);

        // Project to DTO
        var items = await ProjectToReportItem(query)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Calculate status for each item
        await CalculateItemStatusesAsync(items, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("DO status report generated successfully with {Count} records", items.Count);

        return (items, summaryCards, totals);
    }

    private static IQueryable<LoadingRegisterEntity> ApplyFilters(IQueryable<LoadingRegisterEntity> query, DOStatusReportFilter filter)
    {
        if (filter.DateFrom.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate <= filter.DateTo.Value);
        }

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(lr => lr.ConsignorId == filter.CustomerId.Value || lr.ConsigneeId == filter.CustomerId.Value);
        }

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(lr => lr.VehicleId == filter.VehicleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Driver))
        {
            query = query.Where(lr => EF.Functions.Like(lr.Driver, $"%{filter.Driver}%"));
        }

        if (filter.MaterialId.HasValue)
        {
            query = query.Where(lr => lr.MaterialId == filter.MaterialId.Value);
        }

        if (filter.SourceId.HasValue)
        {
            query = query.Where(lr => lr.SourceId == filter.SourceId.Value);
        }

        if (filter.DestinationId.HasValue)
        {
            query = query.Where(lr => lr.DestinationId == filter.DestinationId.Value);
        }

        // Status filters are applied after projection in the main query
        // These will be handled in memory after loading for now, or we can add them to the projection logic

        return query;
    }

    private static IQueryable<LoadingRegisterEntity> ApplySearch(IQueryable<LoadingRegisterEntity> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var searchPattern = $"%{search}%";
        return query.Where(lr =>
            EF.Functions.Like(lr.ChallanNumber, searchPattern) ||
            EF.Functions.Like(lr.TPNumber, searchPattern) ||
            (lr.Vehicle != null && EF.Functions.Like(lr.Vehicle.VehicleNumber, searchPattern)) ||
            (lr.Consignor != null && EF.Functions.Like(lr.Consignor.CustomerName, searchPattern)) ||
            (lr.Consignee != null && EF.Functions.Like(lr.Consignee.CustomerName, searchPattern)) ||
            EF.Functions.Like(lr.Driver, searchPattern) ||
            (lr.Material != null && EF.Functions.Like(lr.Material.MaterialName, searchPattern)));
    }

    private static IQueryable<LoadingRegisterEntity> ApplySorting(IQueryable<LoadingRegisterEntity> query, string? sortBy, bool sortAscending)
    {
        return (sortBy?.ToLower()) switch
        {
            "loadingdate" => sortAscending
                ? query.OrderBy(lr => lr.LoadingDate).ThenBy(lr => lr.ChallanNumber)
                : query.OrderByDescending(lr => lr.LoadingDate).ThenByDescending(lr => lr.ChallanNumber),
            "challannumber" => sortAscending
                ? query.OrderBy(lr => lr.ChallanNumber)
                : query.OrderByDescending(lr => lr.ChallanNumber),
            "vehicle" => sortAscending
                ? query.OrderBy(lr => lr.Vehicle != null ? lr.Vehicle.VehicleNumber : "")
                : query.OrderByDescending(lr => lr.Vehicle != null ? lr.Vehicle.VehicleNumber : ""),
            "customer" => sortAscending
                ? query.OrderBy(lr => lr.Consignor != null ? lr.Consignor.CustomerName : "")
                : query.OrderByDescending(lr => lr.Consignor != null ? lr.Consignor.CustomerName : ""),
            "source" => sortAscending
                ? query.OrderBy(lr => lr.Source != null ? lr.Source.LocationName : "")
                : query.OrderByDescending(lr => lr.Source != null ? lr.Source.LocationName : ""),
            "destination" => sortAscending
                ? query.OrderBy(lr => lr.Destination != null ? lr.Destination.LocationName : "")
                : query.OrderByDescending(lr => lr.Destination != null ? lr.Destination.LocationName : ""),
            "status" => sortAscending
                ? query.OrderBy(lr => lr.LoadingDate)
                : query.OrderByDescending(lr => lr.LoadingDate),
            _ => query.OrderBy(lr => lr.LoadingDate).ThenBy(lr => lr.ChallanNumber)
        };
    }

    private static IQueryable<DOStatusReportItem> ProjectToReportItem(IQueryable<LoadingRegisterEntity> query)
    {
        return query.Select(lr => new DOStatusReportItem
        {
            Id = lr.Id,
            ChallanNumber = lr.ChallanNumber,
            TPNumber = lr.TPNumber,
            LoadingDate = lr.LoadingDate,
            ConsignorName = lr.Consignor != null ? lr.Consignor.CustomerName : null,
            ConsigneeName = lr.Consignee != null ? lr.Consignee.CustomerName : null,
            SourceName = lr.Source != null ? lr.Source.LocationName : null,
            DestinationName = lr.Destination != null ? lr.Destination.LocationName : null,
            VehicleNumber = lr.Vehicle != null ? lr.Vehicle.VehicleNumber : null,
            Driver = lr.Driver,
            MaterialName = lr.Material != null ? lr.Material.MaterialName : null,
            LoadingWeight = lr.LoadingWeight,
            UnloadingWeight = 0, // Will be calculated from unloading register
            ShortageWeight = 0, // Will be calculated from unloading register
            GrossAmount = lr.GrossAmount,
            ChallanMoney = 0, // Will be calculated from unloading register
            PendingAmount = 0, // Will be calculated
            BillNumber = null, // Will be loaded from party bill
            BillDate = null, // Will be loaded from party bill
            DOStatus = DOStatus.Loaded, // Will be calculated
            PaymentStatus = "Pending", // Will be calculated
            BillingStatus = "Pending" // Will be calculated
        });
    }

    private async Task<DOStatusReportSummaryCards> CalculateSummaryCardsAsync(IQueryable<LoadingRegisterEntity> query, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        
        var totalDO = await query.CountAsync(cancellationToken).ConfigureAwait(false);
        var todayLoading = await query.CountAsync(lr => lr.LoadingDate.Date == today, cancellationToken).ConfigureAwait(false);
        
        // Load the data to calculate status-based counts
        var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        
        var loadingIds = items.Select(lr => lr.Id).ToList();
        
        // Get unloading records
        var unloadedIds = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => loadingIds.Contains(ur.LoadingRegisterId ?? 0) && !ur.IsDeleted)
            .Select(ur => ur.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get payment records
        var paidIds = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .Select(pr => pr.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get billed records
        var billedIds = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Where(pbrd => loadingIds.Contains(pbrd.LoadingRegisterId) && !pbrd.IsDeleted)
            .Select(pbrd => pbrd.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        var runningDO = items.Count(lr => !billedIds.Contains(lr.Id));
        var completedDO = items.Count(lr => billedIds.Contains(lr.Id));
        var paymentPending = items.Count(lr => unloadedIds.Contains(lr.Id) && !paidIds.Contains(lr.Id));
        var billPending = items.Count(lr => paidIds.Contains(lr.Id) && !billedIds.Contains(lr.Id));
        
        return new DOStatusReportSummaryCards
        {
            TotalDO = totalDO,
            TodayLoading = todayLoading,
            RunningDO = runningDO,
            CompletedDO = completedDO,
            PaymentPending = paymentPending,
            BillPending = billPending
        };
    }

    private async Task<DOStatusReportTotals> CalculateTotalsAsync(IQueryable<LoadingRegisterEntity> query, CancellationToken cancellationToken)
    {
        var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        var loadingIds = items.Select(lr => lr.Id).ToList();
        
        // Get unloading records for weight calculations
        var unloadings = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => loadingIds.Contains(ur.LoadingRegisterId ?? 0) && !ur.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get payment records for amount calculations
        var payments = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        var totalLoadingWeight = items.Sum(lr => lr.LoadingWeight);
        var totalUnloadingWeight = unloadings.Sum(ur => ur.UnloadingWeight);
        var totalShortageWeight = unloadings.Sum(ur => ur.ShortageWeight);
        var totalGrossAmount = items.Sum(lr => lr.GrossAmount);
        var totalChallanMoney = unloadings.Sum(ur => ur.ChallanMoney);
        var totalPendingAmount = totalGrossAmount - payments.Sum(pr => pr.PayableAmount);
        
        return new DOStatusReportTotals
        {
            TotalRecords = items.Count,
            TotalLoadingWeight = totalLoadingWeight,
            TotalUnloadingWeight = totalUnloadingWeight,
            TotalShortageWeight = totalShortageWeight,
            TotalGrossAmount = totalGrossAmount,
            TotalChallanMoney = totalChallanMoney,
            TotalPendingAmount = totalPendingAmount
        };
    }

    private async Task CalculateItemStatusesAsync(IReadOnlyList<DOStatusReportItem> items, CancellationToken cancellationToken)
    {
        var loadingIds = items.Select(item => item.Id).ToList();
        
        // Get unloading records
        var unloadings = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => loadingIds.Contains(ur.LoadingRegisterId ?? 0) && !ur.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get payment records
        var payments = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        // Get party bill details
        var billDetails = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Where(pbrd => loadingIds.Contains(pbrd.LoadingRegisterId) && !pbrd.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        var billRegisterIds = billDetails.Select(bd => bd.PartyBillRegisterId).Distinct().ToList();
        var billRegisters = await _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Where(pbr => billRegisterIds.Contains(pbr.Id) && !pbr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        
        foreach (var item in items)
        {
            var unloading = unloadings.FirstOrDefault(u => u.LoadingRegisterId == item.Id);
            var payment = payments.FirstOrDefault(p => p.LoadingRegisterId == item.Id);
            var billDetail = billDetails.FirstOrDefault(bd => bd.LoadingRegisterId == item.Id);
            
            // Update unloading weight and shortage
            if (unloading != null)
            {
                item.UnloadingWeight = unloading.UnloadingWeight;
                item.ShortageWeight = unloading.ShortageWeight;
                item.ChallanMoney = unloading.ChallanMoney;
            }
            
            // Update bill information
            if (billDetail != null)
            {
                var billRegister = billRegisters.FirstOrDefault(br => br.Id == billDetail.PartyBillRegisterId);
                if (billRegister != null)
                {
                    item.BillNumber = billRegister.BillNumber;
                    item.BillDate = billRegister.BillDate;
                }
            }
            
            // Calculate pending amount
            if (payment != null)
            {
                item.PendingAmount = item.GrossAmount - payment.PayableAmount;
                item.PaymentStatus = payment.PaymentStatus;
            }
            else
            {
                item.PendingAmount = item.GrossAmount;
                item.PaymentStatus = "Pending";
            }
            
            // Calculate billing status
            item.BillingStatus = billDetail != null ? "Billed" : "Pending";
            
            // Calculate DO status
            item.DOStatus = CalculateDOStatus(unloading, payment, billDetail);
        }
    }

    private static DOStatus CalculateDOStatus(UnloadingRegisterEntity? unloading, PaymentRegisterEntity? payment, PartyBillRegisterDetailEntity? billDetail)
    {
        if (billDetail != null)
        {
            return DOStatus.Completed;
        }
        
        if (payment != null)
        {
            return DOStatus.BillPending;
        }
        
        if (unloading != null)
        {
            return DOStatus.PaymentPending;
        }
        
        return DOStatus.InTransit;
    }
}
