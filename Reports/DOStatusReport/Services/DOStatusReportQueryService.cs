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
    private const int DelayThresholdDays = 3;

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
            DOStatus = DOStatus.InTransit, // Will be calculated
            PaymentStatus = PaymentStatusType.Pending, // Will be calculated
            BillingStatus = BillingStatusType.NotGenerated, // Will be calculated
            ExceptionType = DOExceptionType.None, // Will be calculated
            AgeInDays = 0, // Will be calculated
            IsDelayed = false, // Will be calculated
            DelayDays = 0 // Will be calculated
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

        // Get unloading records with their dates
        var unloadings = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => loadingIds.Contains(ur.LoadingRegisterId ?? 0) && !ur.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var unloadedIds = unloadings.Select(ur => ur.LoadingRegisterId).Distinct().ToHashSet();
        var unloadingByLoadingId = unloadings.ToDictionary(ur => ur.LoadingRegisterId ?? 0);

        // Get payment records
        var payments = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var paidIds = payments.Select(pr => pr.LoadingRegisterId).Distinct().ToHashSet();

        // Get billed records
        var billedIds = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Where(pbrd => loadingIds.Contains(pbrd.LoadingRegisterId) && !pbrd.IsDeleted)
            .Select(pbrd => pbrd.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var billedSet = billedIds.ToHashSet();

        // Calculate basic counts
        var runningDO = items.Count(lr => !billedSet.Contains(lr.Id));
        var completedDO = items.Count(lr => billedSet.Contains(lr.Id));
        var paymentPending = items.Count(lr => unloadedIds.Contains(lr.Id) && !paidIds.Contains(lr.Id));
        var billPending = items.Count(lr => paidIds.Contains(lr.Id) && !billedSet.Contains(lr.Id));

        // Calculate today's completed
        var todayCompleted = items.Count(lr => lr.LoadingDate.Date == today && billedSet.Contains(lr.Id));

        // Calculate delayed DOs (not completed and older than threshold)
        var delayedDO = items.Count(lr =>
        {
            if (billedSet.Contains(lr.Id)) return false;
            var ageInDays = (DateTime.Today - lr.LoadingDate.Date).Days;
            return ageInDays > DelayThresholdDays;
        });

        // Calculate exception DOs
        var exceptionDO = 0;
        foreach (var item in items)
        {
            var hasUnloading = unloadedIds.Contains(item.Id);
            var hasPayment = paidIds.Contains(item.Id);
            var hasBill = billedSet.Contains(item.Id);

            var unloading = hasUnloading ? unloadingByLoadingId.GetValueOrDefault(item.Id) : null;
            var payment = hasPayment ? payments.FirstOrDefault(p => p.LoadingRegisterId == item.Id) : null;

            var exception = DOStatusCalculator.DetectException(
                item.LoadingDate,
                hasUnloading,
                unloading?.UnloadingDate,
                item.LoadingWeight,
                unloading?.UnloadingWeight ?? 0,
                unloading?.ShortageWeight ?? 0,
                item.GrossAmount,
                payment?.PayableAmount,
                hasPayment,
                hasBill,
                DelayThresholdDays);

            if (exception != DOExceptionType.None)
            {
                exceptionDO++;
            }
        }

        // Calculate percentages
        var completionPercentage = totalDO > 0 ? (decimal)completedDO / totalDO * 100 : 0;
        var pendingPercentage = totalDO > 0 ? (decimal)runningDO / totalDO * 100 : 0;

        return new DOStatusReportSummaryCards
        {
            TotalDO = totalDO,
            TodayLoading = todayLoading,
            TodayCompleted = todayCompleted,
            RunningDO = runningDO,
            CompletedDO = completedDO,
            PaymentPending = paymentPending,
            BillPending = billPending,
            DelayedDO = delayedDO,
            ExceptionDO = exceptionDO,
            CompletionPercentage = completionPercentage,
            PendingPercentage = pendingPercentage
        };
    }

    private async Task<DOStatusReportTotals> CalculateTotalsAsync(IQueryable<LoadingRegisterEntity> query, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;
        var items = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        var loadingIds = items.Select(lr => lr.Id).ToList();

        // Get unloading records for weight calculations
        var unloadings = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => loadingIds.Contains(ur.LoadingRegisterId ?? 0) && !ur.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var unloadedIds = unloadings.Select(ur => ur.LoadingRegisterId).Distinct().ToHashSet();

        // Get payment records for amount calculations
        var payments = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var paidIds = payments.Select(pr => pr.LoadingRegisterId).Distinct().ToHashSet();

        // Get billed records
        var billedIds = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Where(pbrd => loadingIds.Contains(pbrd.LoadingRegisterId) && !pbrd.IsDeleted)
            .Select(pbrd => pbrd.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var billedSet = billedIds.ToHashSet();

        // Calculate basic totals
        var totalLoadingWeight = items.Sum(lr => lr.LoadingWeight);
        var totalUnloadingWeight = unloadings.Sum(ur => ur.UnloadingWeight);
        var totalShortageWeight = unloadings.Sum(ur => ur.ShortageWeight);
        var totalGrossAmount = items.Sum(lr => lr.GrossAmount);
        var totalChallanMoney = unloadings.Sum(ur => ur.ChallanMoney);
        var totalPendingAmount = totalGrossAmount - payments.Sum(pr => pr.PayableAmount);

        // Calculate today's values
        var todayItems = items.Where(lr => lr.LoadingDate.Date == today).ToList();
        var todayGrossAmount = todayItems.Sum(lr => lr.GrossAmount);
        var todayLoadingWeight = todayItems.Sum(lr => lr.LoadingWeight);

        // Calculate completed vs pending
        var completedItems = items.Where(lr => billedSet.Contains(lr.Id)).ToList();
        var pendingItems = items.Where(lr => !billedSet.Contains(lr.Id)).ToList();

        var completedGrossAmount = completedItems.Sum(lr => lr.GrossAmount);
        var pendingGrossAmount = pendingItems.Sum(lr => lr.GrossAmount);

        var completedLoadingIds = completedItems.Select(lr => lr.Id).ToHashSet();
        var completedUnloadingWeight = unloadings
            .Where(ur => completedLoadingIds.Contains(ur.LoadingRegisterId ?? 0))
            .Sum(ur => ur.UnloadingWeight);

        var pendingLoadingIds = pendingItems.Select(lr => lr.Id).ToHashSet();
        var pendingUnloadingWeight = unloadings
            .Where(ur => pendingLoadingIds.Contains(ur.LoadingRegisterId ?? 0))
            .Sum(ur => ur.UnloadingWeight);

        return new DOStatusReportTotals
        {
            TotalRecords = items.Count,
            TotalLoadingWeight = totalLoadingWeight,
            TotalUnloadingWeight = totalUnloadingWeight,
            TotalShortageWeight = totalShortageWeight,
            TotalGrossAmount = totalGrossAmount,
            TotalChallanMoney = totalChallanMoney,
            TotalPendingAmount = totalPendingAmount,
            CompletedGrossAmount = completedGrossAmount,
            PendingGrossAmount = pendingGrossAmount,
            TodayGrossAmount = todayGrossAmount,
            TodayLoadingWeight = todayLoadingWeight,
            CompletedLoadingWeight = completedUnloadingWeight,
            PendingLoadingWeight = pendingUnloadingWeight
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

        var unloadingByLoadingId = unloadings.ToDictionary(ur => ur.LoadingRegisterId ?? 0);

        // Get payment records
        var payments = await _dbContext.PaymentRegisters
            .AsNoTracking()
            .Where(pr => loadingIds.Contains(pr.LoadingRegisterId ?? 0) && !pr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var paymentByLoadingId = payments.ToDictionary(pr => pr.LoadingRegisterId ?? 0);

        // Get party bill details
        var billDetails = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Where(pbrd => loadingIds.Contains(pbrd.LoadingRegisterId) && !pbrd.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var billDetailByLoadingId = billDetails.ToDictionary(bd => bd.LoadingRegisterId);

        var billRegisterIds = billDetails.Select(bd => bd.PartyBillRegisterId).Distinct().ToList();
        var billRegisters = await _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Where(pbr => billRegisterIds.Contains(pbr.Id) && !pbr.IsDeleted)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var billRegisterById = billRegisters.ToDictionary(br => br.Id);

        foreach (var item in items)
        {
            var hasUnloading = unloadingByLoadingId.ContainsKey(item.Id);
            var hasPayment = paymentByLoadingId.ContainsKey(item.Id);
            var hasBill = billDetailByLoadingId.ContainsKey(item.Id);

            var unloading = hasUnloading ? unloadingByLoadingId[item.Id] : null;
            var payment = hasPayment ? paymentByLoadingId[item.Id] : null;
            var billDetail = hasBill ? billDetailByLoadingId[item.Id] : null;

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
                var billRegister = billRegisterById.GetValueOrDefault(billDetail.PartyBillRegisterId);
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
            }
            else
            {
                item.PendingAmount = item.GrossAmount;
            }

            // Use centralized calculator for all status calculations
            item.DOStatus = DOStatusCalculator.CalculateDOStatus(hasUnloading, hasPayment, hasBill);
            item.PaymentStatus = DOStatusCalculator.ConvertPaymentStatus(payment?.PaymentStatus);
            item.BillingStatus = DOStatusCalculator.ConvertBillingStatus(hasBill, null);

            // Calculate exception
            item.ExceptionType = DOStatusCalculator.DetectException(
                item.LoadingDate,
                hasUnloading,
                unloading?.UnloadingDate,
                item.LoadingWeight,
                item.UnloadingWeight,
                item.ShortageWeight,
                item.GrossAmount,
                payment?.PayableAmount,
                hasPayment,
                hasBill,
                DelayThresholdDays);

            // Calculate age and delay
            item.AgeInDays = DOStatusCalculator.CalculateAgeInDays(item.LoadingDate);
            var (isDelayed, delayDays) = DOStatusCalculator.CalculateDelay(item.LoadingDate, hasBill, DelayThresholdDays);
            item.IsDelayed = isDelayed;
            item.DelayDays = delayDays;
        }
    }
}
