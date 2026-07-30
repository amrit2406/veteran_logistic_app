using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using veteran_logistic.Reports.LoadingReport.Contracts;
using veteran_logistic.Reports.LoadingReport.DTOs;

namespace veteran_logistic.Reports.LoadingReport.Services;

/// <summary>
/// Implementation of the loading report query service.
/// </summary>
public sealed class LoadingReportQueryService : ILoadingReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<LoadingReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public LoadingReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<LoadingReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<LoadingReportItem> Items, LoadingReportTotals Totals)> GetLoadingReportAsync(
        LoadingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating loading report with filters and search");

        var query = _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => !lr.IsDeleted);

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

        _logger.LogInformation("Loading report generated successfully with {Count} records", items.Count);

        return (items, totals);
    }

    private static IQueryable<LoadingRegisterEntity> ApplyFilters(IQueryable<LoadingRegisterEntity> query, LoadingReportFilter filter)
    {
        if (filter.DateFrom.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate <= filter.DateTo.Value);
        }

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

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(lr => lr.VehicleId == filter.VehicleId.Value);
        }

        if (filter.MaterialId.HasValue)
        {
            query = query.Where(lr => lr.MaterialId == filter.MaterialId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Driver))
        {
            query = query.Where(lr => EF.Functions.Like(lr.Driver, $"%{filter.Driver}%"));
        }

        if (filter.OwnerId.HasValue)
        {
            query = query.Where(lr => lr.OwnerId == filter.OwnerId.Value);
        }

        if (filter.UnionVendorId.HasValue)
        {
            query = query.Where(lr => lr.UnionVendorId == filter.UnionVendorId.Value);
        }

        if (filter.PaymentLocationId.HasValue)
        {
            query = query.Where(lr => lr.PaymentLocationId == filter.PaymentLocationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ChallanNumber))
        {
            query = query.Where(lr => EF.Functions.Like(lr.ChallanNumber, $"%{filter.ChallanNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.TPNumber))
        {
            query = query.Where(lr => EF.Functions.Like(lr.TPNumber, $"%{filter.TPNumber}%"));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(lr => lr.IsActive == filter.IsActive.Value);
        }

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
            "material" => sortAscending
                ? query.OrderBy(lr => lr.Material != null ? lr.Material.MaterialName : "")
                : query.OrderByDescending(lr => lr.Material != null ? lr.Material.MaterialName : ""),
            "grossweight" => sortAscending
                ? query.OrderBy(lr => lr.GrossWeight)
                : query.OrderByDescending(lr => lr.GrossWeight),
            "loadingweight" => sortAscending
                ? query.OrderBy(lr => lr.LoadingWeight)
                : query.OrderByDescending(lr => lr.LoadingWeight),
            "grossamount" => sortAscending
                ? query.OrderBy(lr => lr.GrossAmount)
                : query.OrderByDescending(lr => lr.GrossAmount),
            _ => query.OrderBy(lr => lr.LoadingDate).ThenBy(lr => lr.ChallanNumber)
        };
    }

    private static IQueryable<LoadingReportItem> ProjectToReportItem(IQueryable<LoadingRegisterEntity> query)
    {
        return query.Select(lr => new LoadingReportItem
        {
            Id = lr.Id,
            ChallanNumber = lr.ChallanNumber,
            LoadingDate = lr.LoadingDate,
            CompanyName = null, // TODO: Add Company relationship when available
            CustomerName = lr.Consignor != null ? lr.Consignor.CustomerName : null,
            ConsignorName = lr.Consignor != null ? lr.Consignor.CustomerName : null,
            ConsigneeName = lr.Consignee != null ? lr.Consignee.CustomerName : null,
            SourceName = lr.Source != null ? lr.Source.LocationName : null,
            DestinationName = lr.Destination != null ? lr.Destination.LocationName : null,
            VehicleNumber = lr.Vehicle != null ? lr.Vehicle.VehicleNumber : null,
            Driver = lr.Driver,
            MaterialName = lr.Material != null ? lr.Material.MaterialName : null,
            GrossWeight = lr.GrossWeight,
            TareWeight = lr.TareWeight,
            LoadingWeight = lr.LoadingWeight,
            Rate = lr.Rate,
            GrossAmount = lr.GrossAmount,
            FuelAmount = lr.FuelAmount,
            CashAdvance = lr.CashAdvance,
            OtherAdvance = lr.OtherAdvance,
            PaymentLocationName = lr.PaymentLocation != null ? lr.PaymentLocation.PaymentLocationName : null,
            UnionVendorName = lr.UnionVendor != null ? lr.UnionVendor.Name : null,
            OwnerName = lr.Owner != null ? (!string.IsNullOrWhiteSpace(lr.Owner.CompanyName) ? lr.Owner.CompanyName : $"{lr.Owner.FirstName} {lr.Owner.LastName}".Trim()) : null,
            TPNumber = lr.TPNumber,
            IsActive = lr.IsActive
        });
    }

    private static async Task<LoadingReportTotals> CalculateTotalsAsync(IQueryable<LoadingRegisterEntity> query, CancellationToken cancellationToken)
    {
        var totals = await query
            .GroupBy(lr => 1)
            .Select(g => new LoadingReportTotals
            {
                RecordCount = g.Count(),
                TotalGrossWeight = g.Sum(lr => lr.GrossWeight),
                TotalTareWeight = g.Sum(lr => lr.TareWeight),
                TotalLoadingWeight = g.Sum(lr => lr.LoadingWeight),
                TotalGrossAmount = g.Sum(lr => lr.GrossAmount),
                TotalFuelAmount = g.Sum(lr => lr.FuelAmount),
                TotalCashAdvance = g.Sum(lr => lr.CashAdvance),
                TotalOtherAdvance = g.Sum(lr => lr.OtherAdvance)
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return totals ?? new LoadingReportTotals();
    }
}
