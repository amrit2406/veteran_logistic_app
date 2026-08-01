using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PartyBillRegisterEntity = VeteranLogistics.Data.Entities.Administration.PartyBillRegister;
using PartyBillRegisterDetailEntity = VeteranLogistics.Data.Entities.Administration.PartyBillRegisterDetail;
using veteran_logistic.Reports.PartyBillingReport.Contracts;
using veteran_logistic.Reports.PartyBillingReport.DTOs;

namespace veteran_logistic.Reports.PartyBillingReport.Services;

/// <summary>
/// Implementation of the party billing report query service.
/// </summary>
public sealed class PartyBillingReportQueryService : IPartyBillingReportQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PartyBillingReportQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillingReportQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PartyBillingReportQueryService(VeteranLogisticsDbContext dbContext, ILogger<PartyBillingReportQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<(IReadOnlyList<PartyBillingReportItem> Items, PartyBillingReportTotals Totals)> GetPartyBillingReportAsync(
        PartyBillingReportFilter filter,
        string? search,
        string? sortBy,
        bool sortAscending,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating party billing report summary with filters and search");

        var query = _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Include(pbr => pbr.Party)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Vehicle)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Material)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Consignor)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Consignee)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Source)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
                    .ThenInclude(lr => lr!.Destination)
            .Where(pbr => !pbr.IsDeleted);

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

        _logger.LogInformation("Party billing report summary generated successfully with {Count} records", items.Count);

        return (items, totals);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PartyBillingReportDetailItem>> GetPartyBillingReportDetailsAsync(
        int partyBillRegisterId,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating party billing report details for bill ID {BillId}", partyBillRegisterId);

        var details = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Vehicle)
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Material)
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Consignor)
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Consignee)
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Source)
            .Include(pbrd => pbrd.LoadingRegister)
                .ThenInclude(lr => lr!.Destination)
            .Where(pbrd => pbrd.PartyBillRegisterId == partyBillRegisterId && !pbrd.IsDeleted)
            .Select(pbrd => new PartyBillingReportDetailItem
            {
                Id = pbrd.Id,
                PartyBillRegisterId = pbrd.PartyBillRegisterId,
                ChallanNumber = pbrd.ChallanNumber,
                LoadingDate = pbrd.LoadingDate,
                VehicleNumber = pbrd.VehicleNumber,
                Material = pbrd.LoadingRegister != null && pbrd.LoadingRegister.Material != null
                    ? pbrd.LoadingRegister.Material.MaterialName
                    : null,
                Consignor = pbrd.LoadingRegister != null && pbrd.LoadingRegister.Consignor != null
                    ? pbrd.LoadingRegister.Consignor.CustomerName
                    : null,
                Consignee = pbrd.LoadingRegister != null && pbrd.LoadingRegister.Consignee != null
                    ? pbrd.LoadingRegister.Consignee.CustomerName
                    : null,
                Source = pbrd.LoadingRegister != null && pbrd.LoadingRegister.Source != null
                    ? pbrd.LoadingRegister.Source.LocationName
                    : null,
                Destination = pbrd.LoadingRegister != null && pbrd.LoadingRegister.Destination != null
                    ? pbrd.LoadingRegister.Destination.LocationName
                    : null,
                LoadingWeight = pbrd.MaterialWeight,
                Rate = pbrd.BillingRate,
                GrossAmount = pbrd.Amount
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Party billing report details generated successfully with {Count} records", details.Count);

        return details;
    }

    private static IQueryable<PartyBillRegisterEntity> ApplyFilters(IQueryable<PartyBillRegisterEntity> query, PartyBillingReportFilter filter)
    {
        if (filter.BillDateFrom.HasValue)
        {
            query = query.Where(pbr => pbr.BillDate >= filter.BillDateFrom.Value);
        }

        if (filter.BillDateTo.HasValue)
        {
            query = query.Where(pbr => pbr.BillDate <= filter.BillDateTo.Value);
        }

        if (filter.LoadingDateFrom.HasValue || filter.LoadingDateTo.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted &&
                    (!filter.LoadingDateFrom.HasValue || pbrd.LoadingDate >= filter.LoadingDateFrom.Value) &&
                    (!filter.LoadingDateTo.HasValue || pbrd.LoadingDate <= filter.LoadingDateTo.Value)));
        }

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyId == filter.CustomerId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.ThirdParty))
        {
            query = query.Where(pbr => EF.Functions.Like(pbr.ThirdPartyName, $"%{filter.ThirdParty}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.PermitNumber))
        {
            query = query.Where(pbr => EF.Functions.Like(pbr.PermitNumber ?? "", $"%{filter.PermitNumber}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.BillNumber))
        {
            query = query.Where(pbr => EF.Functions.Like(pbr.BillNumber, $"%{filter.BillNumber}%"));
        }

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted && pbrd.LoadingRegister != null && pbrd.LoadingRegister.VehicleId == filter.VehicleId.Value));
        }

        if (filter.MaterialId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted && pbrd.LoadingRegister != null && pbrd.LoadingRegister.MaterialId == filter.MaterialId.Value));
        }

        if (filter.ConsignorId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted && pbrd.LoadingRegister != null && pbrd.LoadingRegister.ConsignorId == filter.ConsignorId.Value));
        }

        if (filter.ConsigneeId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted && pbrd.LoadingRegister != null && pbrd.LoadingRegister.ConsigneeId == filter.ConsigneeId.Value));
        }

        if (filter.DestinationId.HasValue)
        {
            query = query.Where(pbr => pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted && pbrd.LoadingRegister != null && pbrd.LoadingRegister.DestinationId == filter.DestinationId.Value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(pbr => EF.Functions.Like(pbr.IsActive ? "Active" : "Inactive", $"%{filter.Status}%"));
        }

        return query;
    }

    private static IQueryable<PartyBillRegisterEntity> ApplySearch(IQueryable<PartyBillRegisterEntity> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return query;
        }

        var searchPattern = $"%{search}%";
        return query.Where(pbr =>
            EF.Functions.Like(pbr.BillNumber, searchPattern) ||
            (pbr.Party != null && EF.Functions.Like(pbr.Party.CustomerName, searchPattern)) ||
            EF.Functions.Like(pbr.ThirdPartyName, searchPattern) ||
            EF.Functions.Like(pbr.PermitNumber ?? "", searchPattern) ||
            pbr.PartyBillRegisterDetails
                .Any(pbrd => !pbrd.IsDeleted &&
                    (EF.Functions.Like(pbrd.ChallanNumber, searchPattern) ||
                    EF.Functions.Like(pbrd.VehicleNumber ?? "", searchPattern) ||
                    (pbrd.LoadingRegister != null && pbrd.LoadingRegister.Material != null && EF.Functions.Like(pbrd.LoadingRegister.Material.MaterialName, searchPattern)) ||
                    (pbrd.LoadingRegister != null && pbrd.LoadingRegister.Consignor != null && EF.Functions.Like(pbrd.LoadingRegister.Consignor.CustomerName, searchPattern)) ||
                    (pbrd.LoadingRegister != null && pbrd.LoadingRegister.Consignee != null && EF.Functions.Like(pbrd.LoadingRegister.Consignee.CustomerName, searchPattern)) ||
                    (pbrd.LoadingRegister != null && pbrd.LoadingRegister.Destination != null && EF.Functions.Like(pbrd.LoadingRegister.Destination.LocationName, searchPattern)))));
    }

    private static IQueryable<PartyBillRegisterEntity> ApplySorting(IQueryable<PartyBillRegisterEntity> query, string? sortBy, bool sortAscending)
    {
        var isAscending = sortAscending ? "ThenBy" : "ThenByDescending";

        return sortBy?.ToLowerInvariant() switch
        {
            "billdate" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.BillDate)
                : query.OrderByDescending(pbr => pbr.BillDate),
            "billnumber" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.BillNumber)
                : query.OrderByDescending(pbr => pbr.BillNumber),
            "customer" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.Party != null ? pbr.Party.CustomerName : "")
                : query.OrderByDescending(pbr => pbr.Party != null ? pbr.Party.CustomerName : ""),
            "thirdparty" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.ThirdPartyName)
                : query.OrderByDescending(pbr => pbr.ThirdPartyName),
            "totalchallans" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.TotalRecords)
                : query.OrderByDescending(pbr => pbr.TotalRecords),
            "totalweight" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.TotalMaterialWeight)
                : query.OrderByDescending(pbr => pbr.TotalMaterialWeight),
            "totalamount" => isAscending == "ThenBy"
                ? query.OrderBy(pbr => pbr.GrandTotal)
                : query.OrderByDescending(pbr => pbr.GrandTotal),
            _ => query.OrderByDescending(pbr => pbr.BillDate)
        };
    }

    private static async Task<PartyBillingReportTotals> CalculateTotalsAsync(IQueryable<PartyBillRegisterEntity> query, CancellationToken cancellationToken)
    {
        var records = await query.ToListAsync(cancellationToken).ConfigureAwait(false);

        return new PartyBillingReportTotals
        {
            RecordCount = records.Count,
            TotalBills = records.Count,
            TotalChallans = records.Sum(pbr => pbr.TotalRecords),
            TotalLoadingWeight = records.Sum(pbr => pbr.TotalMaterialWeight),
            TotalGrossAmount = records.Sum(pbr => pbr.GrandTotal),
            AverageBillAmount = records.Any() ? records.Average(pbr => pbr.GrandTotal) : 0
        };
    }

    private static IQueryable<PartyBillingReportItem> ProjectToReportItem(IQueryable<PartyBillRegisterEntity> query)
    {
        return query.Select(pbr => new PartyBillingReportItem
        {
            Id = pbr.Id,
            BillNumber = pbr.BillNumber,
            BillDate = pbr.BillDate,
            Customer = pbr.Party != null ? pbr.Party.CustomerName : string.Empty,
            ThirdParty = pbr.ThirdPartyName,
            PermitNumber = pbr.PermitNumber,
            FromDate = pbr.FromDate,
            ToDate = pbr.ToDate,
            NumberOfChallans = pbr.TotalRecords,
            TotalLoadingWeight = pbr.TotalMaterialWeight,
            TotalBillAmount = pbr.GrandTotal,
            Status = pbr.IsActive ? "Active" : "Inactive"
        });
    }
}
