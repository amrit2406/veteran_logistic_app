using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using veteran_logistic.Reports.UnloadingReport.Contracts;
using veteran_logistic.Reports.UnloadingReport.DTOs;

namespace veteran_logistic.Reports.UnloadingReport.Services;

/// <summary>
/// Implementation of the unloading report query service.
/// </summary>
public sealed class UnloadingReportQueryService : IUnloadingReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<UnloadingReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnloadingReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public UnloadingReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<UnloadingReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<UnloadingReportItem> Items, UnloadingReportTotals Totals)> GetUnloadingReportAsync(
        UnloadingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating unloading report with filters and search");

        var query = _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => !ur.IsDeleted);

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

        _logger.LogInformation("Unloading report generated successfully with {Count} records", items.Count);

        return (items, totals);
    }

    private static IQueryable<UnloadingRegisterEntity> ApplyFilters(IQueryable<UnloadingRegisterEntity> query, UnloadingReportFilter filter)
    {
        if (filter.DateFrom.HasValue)
        {
            query = query.Where(ur => ur.UnloadingDate >= filter.DateFrom.Value);
        }

        if (filter.DateTo.HasValue)
        {
            query = query.Where(ur => ur.UnloadingDate <= filter.DateTo.Value);
        }

        if (filter.ConsignorId.HasValue)
        {
            query = query.Where(ur => ur.ConsignorId == filter.ConsignorId.Value);
        }

        if (filter.ConsigneeId.HasValue)
        {
            query = query.Where(ur => ur.ConsigneeId == filter.ConsigneeId.Value);
        }

        if (filter.SourceId.HasValue)
        {
            query = query.Where(ur => ur.SourceId == filter.SourceId.Value);
        }

        if (filter.DestinationId.HasValue)
        {
            query = query.Where(ur => ur.DestinationId == filter.DestinationId.Value);
        }

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(ur => ur.VehicleId == filter.VehicleId.Value);
        }

        if (filter.MaterialId.HasValue)
        {
            query = query.Where(ur => ur.MaterialId == filter.MaterialId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Driver))
        {
            query = query.Where(ur => EF.Functions.Like(ur.Driver, $"%{filter.Driver}%"));
        }

        if (filter.OwnerId.HasValue)
        {
            query = query.Where(ur => ur.OwnerId == filter.OwnerId.Value);
        }

        if (filter.UnionVendorId.HasValue)
        {
            query = query.Where(ur => ur.UnionVendorId == filter.UnionVendorId.Value);
        }

        if (filter.PaymentLocationId.HasValue)
        {
            query = query.Where(ur => ur.PaymentLocationId == filter.PaymentLocationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ChallanNumber))
        {
            query = query.Where(ur => EF.Functions.Like(ur.ChallanNumber, $"%{filter.ChallanNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.TPNumber))
        {
            query = query.Where(ur => EF.Functions.Like(ur.TPNumber, $"%{filter.TPNumber}%"));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(ur => ur.IsActive == filter.IsActive.Value);
        }

        return query;
    }

    private static IQueryable<UnloadingRegisterEntity> ApplySearch(IQueryable<UnloadingRegisterEntity> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var searchPattern = $"%{search}%";
        return query.Where(ur =>
            EF.Functions.Like(ur.ChallanNumber, searchPattern) ||
            EF.Functions.Like(ur.TPNumber, searchPattern) ||
            (ur.Vehicle != null && EF.Functions.Like(ur.Vehicle.VehicleNumber, searchPattern)) ||
            (ur.Consignor != null && EF.Functions.Like(ur.Consignor.CustomerName, searchPattern)) ||
            (ur.Consignee != null && EF.Functions.Like(ur.Consignee.CustomerName, searchPattern)) ||
            EF.Functions.Like(ur.Driver, searchPattern) ||
            (ur.Material != null && EF.Functions.Like(ur.Material.MaterialName, searchPattern)) ||
            (ur.Owner != null && EF.Functions.Like(ur.Owner.CompanyName, searchPattern)));
    }

    private static IQueryable<UnloadingRegisterEntity> ApplySorting(IQueryable<UnloadingRegisterEntity> query, string? sortBy, bool sortAscending)
    {
        return (sortBy?.ToLower()) switch
        {
            "unloadingdate" => sortAscending
                ? query.OrderBy(ur => ur.UnloadingDate).ThenBy(ur => ur.ChallanNumber)
                : query.OrderByDescending(ur => ur.UnloadingDate).ThenByDescending(ur => ur.ChallanNumber),
            "challannumber" => sortAscending
                ? query.OrderBy(ur => ur.ChallanNumber)
                : query.OrderByDescending(ur => ur.ChallanNumber),
            "vehicle" => sortAscending
                ? query.OrderBy(ur => ur.Vehicle != null ? ur.Vehicle.VehicleNumber : "")
                : query.OrderByDescending(ur => ur.Vehicle != null ? ur.Vehicle.VehicleNumber : ""),
            "customer" => sortAscending
                ? query.OrderBy(ur => ur.Consignor != null ? ur.Consignor.CustomerName : "")
                : query.OrderByDescending(ur => ur.Consignor != null ? ur.Consignor.CustomerName : ""),
            "material" => sortAscending
                ? query.OrderBy(ur => ur.Material != null ? ur.Material.MaterialName : "")
                : query.OrderByDescending(ur => ur.Material != null ? ur.Material.MaterialName : ""),
            "grossweight" => sortAscending
                ? query.OrderBy(ur => ur.GrossWeight)
                : query.OrderByDescending(ur => ur.GrossWeight),
            "unloadingweight" => sortAscending
                ? query.OrderBy(ur => ur.UnloadingWeight)
                : query.OrderByDescending(ur => ur.UnloadingWeight),
            "shortageweight" => sortAscending
                ? query.OrderBy(ur => ur.ShortageWeight)
                : query.OrderByDescending(ur => ur.ShortageWeight),
            "grossamount" => sortAscending
                ? query.OrderBy(ur => ur.GrossAmount)
                : query.OrderByDescending(ur => ur.GrossAmount),
            _ => query.OrderBy(ur => ur.UnloadingDate).ThenBy(ur => ur.ChallanNumber)
        };
    }

    private static IQueryable<UnloadingReportItem> ProjectToReportItem(IQueryable<UnloadingRegisterEntity> query)
    {
        return query.Select(ur => new UnloadingReportItem
        {
            Id = ur.Id,
            ChallanNumber = ur.ChallanNumber,
            UnloadingDate = ur.UnloadingDate,
            CompanyName = null, // TODO: Add Company relationship when available
            CustomerName = ur.Consignor != null ? ur.Consignor.CustomerName : null,
            ConsignorName = ur.Consignor != null ? ur.Consignor.CustomerName : null,
            ConsigneeName = ur.Consignee != null ? ur.Consignee.CustomerName : null,
            SourceName = ur.Source != null ? ur.Source.LocationName : null,
            DestinationName = ur.Destination != null ? ur.Destination.LocationName : null,
            VehicleNumber = ur.Vehicle != null ? ur.Vehicle.VehicleNumber : null,
            Driver = ur.Driver,
            MaterialName = ur.Material != null ? ur.Material.MaterialName : null,
            GrossWeight = ur.GrossWeight,
            TareWeight = ur.TareWeight,
            UnloadingWeight = ur.UnloadingWeight,
            ShortageWeight = ur.ShortageWeight,
            Rate = ur.Rate,
            GrossAmount = ur.GrossAmount,
            FuelAmount = ur.FuelAmount,
            CashAdvance = ur.CashAdvance,
            OtherAdvance = ur.OtherAdvance,
            PaymentLocationName = ur.PaymentLocation != null ? ur.PaymentLocation.PaymentLocationName : null,
            UnionVendorName = ur.UnionVendor != null ? ur.UnionVendor.Name : null,
            OwnerName = ur.Owner != null ? (!string.IsNullOrWhiteSpace(ur.Owner.CompanyName) ? ur.Owner.CompanyName : $"{ur.Owner.FirstName} {ur.Owner.LastName}".Trim()) : null,
            TPNumber = ur.TPNumber,
            IsActive = ur.IsActive
        });
    }

    private static async Task<UnloadingReportTotals> CalculateTotalsAsync(IQueryable<UnloadingRegisterEntity> query, CancellationToken cancellationToken)
    {
        var totals = await query
            .GroupBy(ur => 1)
            .Select(g => new UnloadingReportTotals
            {
                RecordCount = g.Count(),
                TotalGrossWeight = g.Sum(ur => ur.GrossWeight),
                TotalTareWeight = g.Sum(ur => ur.TareWeight),
                TotalUnloadingWeight = g.Sum(ur => ur.UnloadingWeight),
                TotalShortageWeight = g.Sum(ur => ur.ShortageWeight),
                TotalGrossAmount = g.Sum(ur => ur.GrossAmount),
                TotalFuelAmount = g.Sum(ur => ur.FuelAmount),
                TotalCashAdvance = g.Sum(ur => ur.CashAdvance),
                TotalOtherAdvance = g.Sum(ur => ur.OtherAdvance)
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        return totals ?? new UnloadingReportTotals();
    }
}
